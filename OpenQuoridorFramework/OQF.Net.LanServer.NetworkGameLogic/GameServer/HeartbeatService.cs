using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.Types;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.Messaging;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	internal class HeartbeatService : DisposingObject
	{
		private class ReceivedHeartBeatCollection
		{
			public ReceivedHeartBeatCollection()
			{
				Receivements = new ConcurrentDictionary<ClientId, bool>();
			}

			public IDictionary<ClientId, bool> Receivements { get; }
		}

		public event Action<ClientId> ClientVanished;

		private readonly IServerMessaging messagingService;
		private readonly IClientRepository clientRepository;
		
		private readonly Timer sendingTimer;
		private readonly Timer receivingTimer;

		private readonly ReceivedHeartBeatCollection receivedHeartBeatCollection;

		public HeartbeatService(IServerMessaging messagingService, IClientRepository clientRepository)
		{
			this.messagingService = messagingService;
			this.clientRepository = clientRepository;

			receivedHeartBeatCollection = new ReceivedHeartBeatCollection();
			OnRepositoryChanged();

			clientRepository.RepositoryChanged += OnRepositoryChanged;
			messagingService.NewIncomingMessage += OnNewIncomingMessage;			


			sendingTimer   = new Timer(OnSendingTimerTick, null, TimeSpan.FromMilliseconds( 500), TimeSpan.FromMilliseconds(2000));
			receivingTimer = new Timer(OnReceivingTimer,   null, TimeSpan.FromMilliseconds(5000), TimeSpan.FromMilliseconds(5000));			
		}

		private void OnRepositoryChanged()
		{
			foreach (var clientId in clientRepository.GetAllClients()
													 .Select(clientInfo => clientInfo.ClientId)
													 .Where(clientId => !receivedHeartBeatCollection.Receivements.ContainsKey(clientId)))
			{
				receivedHeartBeatCollection.Receivements.Add(clientId, true);
			}

			foreach (var clientId in receivedHeartBeatCollection.Receivements
																.Keys
																.Where(clientId => !clientRepository.IsClientIdRegistered(clientId)))
			{
				receivedHeartBeatCollection.Receivements.Remove(clientId);
			}
		}

		private void OnReceivingTimer(object state)
		{
			var currentReceivments = receivedHeartBeatCollection.Receivements.ToDictionary(pair => pair.Key, pair => pair.Value);

			foreach (var receivementPair in currentReceivments)
			{				
				if (!receivementPair.Value)
				{
					ClientVanished?.Invoke(receivementPair.Key);
					receivedHeartBeatCollection.Receivements.Remove(receivementPair.Key);
				}
				else
				{
					receivedHeartBeatCollection.Receivements[receivementPair.Key] = false;
				}
			}			
		}

		private void OnSendingTimerTick(object state)
		{
			if (clientRepository.GetAllClients().Any())
			{
				messagingService.SendMessage(new HeartBeat(new ClientId(Guid.Empty)));
			}
				
		}

		private void OnNewIncomingMessage(NetworkMessageBase incommingMsg)
		{
			if (incommingMsg.Type == NetworkMessageType.HeartBeat)
			{
				receivedHeartBeatCollection.Receivements[incommingMsg.ClientId] = true;
			}
		}

		private void StopTimers()
		{
			sendingTimer.Change(Timeout.Infinite, Timeout.Infinite);
			receivingTimer.Change(Timeout.Infinite, Timeout.Infinite);			
		}

		protected override void CleanUp()
		{
			StopTimers();

			sendingTimer.Dispose();
			receivingTimer.Dispose();

			messagingService.NewIncomingMessage -= OnNewIncomingMessage;
		}
	}
}
