using System;
using System.Threading;
using Lib.FrameworkExtension;
using OQF.Net.DesktopClient.NetworkGameLogic.Messaging;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.NetworkGameLogic
{
	internal class HeartbeatService : DisposingObject
	{
		public event Action ServerVanished;

		private readonly IClientMessaging messagingService;
		private readonly ClientId clientId;
		private readonly Timer sendingTimer;
		private readonly Timer receivingTimer;


		public HeartbeatService(IClientMessaging messagingService, ClientId clientId)
		{
			this.messagingService = messagingService;
			this.clientId = clientId;

			messagingService.NewIncomingMessage += OnNewIncomingMessage;

			sendingTimer   = new Timer(OnSendingTimerTick, null, TimeSpan.FromMilliseconds( 500), TimeSpan.FromMilliseconds(2000));
			receivingTimer = new Timer(OnReceivingTimer,   null, TimeSpan.FromMilliseconds(5000), TimeSpan.FromMilliseconds(5000));

			receivedHeartBeat = false;
		}

		private volatile bool receivedHeartBeat;

		private void OnReceivingTimer(object state)
		{
			if (!receivedHeartBeat)
			{
				StopTimers();
				ServerVanished?.Invoke();
			}
			else
			{
				receivedHeartBeat = false;
			}
		}

		private void OnSendingTimerTick(object state)
		{
			messagingService.SendMessage(new HeartBeat(clientId));
		}

		private void OnNewIncomingMessage(NetworkMessageBase incommingMsg)
		{
			if (incommingMsg.Type == NetworkMessageType.HeartBeat)
			{
				receivedHeartBeat = true;
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
