using System;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.Messaging;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class NetworkGameServer : INetworkGameServer
	{
		public event Action<string> NewOutputAvailable;

		private IServerMessaging messagingService;

		private readonly IClientRepository clientRepository;

		public NetworkGameServer(IClientRepository clientRepository)
		{
			this.clientRepository = clientRepository;
		}


		public void Activate(Address serverAddress)
		{
			if (messagingService != null)
			{
				Deactivate();	
			}

			clientRepository.ClearRepository();
			messagingService = new ServerMessaging(serverAddress);

			messagingService.NewIncomingMessage += OnNewIncomingMessage;
		}

		private void OnNewIncomingMessage(NetworkMessageBase networkMessageBase)
		{
			// TODO: handle messages
		}

		public void Deactivate()
		{
			if (messagingService != null)
			{
				messagingService.NewIncomingMessage -= OnNewIncomingMessage;
				messagingService.Dispose();
				messagingService = null;
			}
		}
	}
}
