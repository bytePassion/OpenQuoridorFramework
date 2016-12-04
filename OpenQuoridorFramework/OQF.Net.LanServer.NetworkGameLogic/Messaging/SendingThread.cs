using System;
using Lib.Concurrency;
using NetMQ.Sockets;
using OQF.Net.LanMessaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.SendReceive;

namespace OQF.Net.LanServer.NetworkGameLogic.Messaging
{
	internal class SendingThread : IThread
	{
		public event Action<NetworkMessageBase> SendingError;

		private readonly Address serverAddress;
		private readonly TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue;
		private volatile bool stopRunning;

		public SendingThread(Address serverAddress,
							 TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue)
		{
			this.serverAddress = serverAddress;
			this.outgoingMessageQueue = outgoingMessageQueue;
			stopRunning = false;
			IsRunning = false;
		}

		public void Run()
		{
			IsRunning = true;
			using (var socket = new PublisherSocket())
			{
				socket.Options.Linger = TimeSpan.Zero;
				socket.Bind(serverAddress.ZmqAddress + ":" + MessagingConstants.TcpIpPort.PubSubPort);

				while (!stopRunning)
				{
					var nextMessage = outgoingMessageQueue.TimeoutTake();

					if (nextMessage == null)
						continue;

					var wasSendingSuccessful = socket.SendNetworkMsg(nextMessage);

					if (!wasSendingSuccessful)
					{
						SendingError?.Invoke(nextMessage);
					}
				}
			}

			outgoingMessageQueue.Dispose(); // It's easier to do this here than in the ThreadCollection			
			IsRunning = false;
		}

		public void Stop()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}