using System;
using Lib.Concurrency;
using NetMQ.Sockets;
using OQF.Net.LanMessaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.SendReceive;

namespace OQF.Net.DesktopClient.NetworkGameLogic.Messaging
{
	internal class SendingThread : IThread
	{
		private readonly Address serverAddress;
		private readonly TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue;

		private volatile bool stopRunning;

		public SendingThread(Address serverAddress, TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue)
		{
			this.serverAddress = serverAddress;
			this.outgoingMessageQueue = outgoingMessageQueue;
		}

		public void Run()
		{
			IsRunning = true;
			using (var socket = new PushSocket())
			{
				socket.Options.Linger = TimeSpan.Zero;
				socket.Connect(serverAddress.ZmqAddress + ":" + MessagingConstants.TcpIpPort.PushPullPort);

				while (!stopRunning)
				{
					var nextMessage = outgoingMessageQueue.TimeoutTake();

					if (nextMessage == null)
						continue;

					var wasSendingSuccessful = socket.SendNetworkMsg(nextMessage);

					if (!wasSendingSuccessful)
					{
						// TODO report error
					}
				}
			}

			outgoingMessageQueue.Dispose(); // It's easier to do this here than in the ThreadCollection			
			IsRunning = false;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}