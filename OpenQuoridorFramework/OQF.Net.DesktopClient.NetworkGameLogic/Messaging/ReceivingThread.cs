using System;
using Lib.Concurrency;
using NetMQ.Sockets;
using OQF.Net.LanMessaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.SendReceive;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.NetworkGameLogic.Messaging
{
	internal class ReceivingThread : IThread
	{
		public event Action<NetworkMessageBase> NewMessageAvailable;

		private readonly Address serverAddress;
		private readonly ClientId myClientId;
		private volatile bool stopRunning;

		public ReceivingThread(Address serverAddress, ClientId myClientId)
		{
			this.serverAddress = serverAddress;
			this.myClientId = myClientId;
		}

		public void Run()
		{
			IsRunning = true;
			using (var socket = new SubscriberSocket())
			{
				socket.SubscribeToAnyTopic();
				socket.Options.Linger = TimeSpan.Zero;
				socket.Connect(serverAddress.ZmqAddress + ":" + MessagingConstants.TcpIpPort.PubSubPort);

				while(!stopRunning)
				{
					var incommingMessage = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (incommingMessage == null)
						continue;

					var isMessageForAllClients = incommingMessage.ClientId == new ClientId(Guid.Empty);

					if (incommingMessage.ClientId != myClientId && !isMessageForAllClients)
					{
						continue;
					}
						
					NewMessageAvailable?.Invoke(incommingMessage);
				}
			}
			
			IsRunning = false;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}