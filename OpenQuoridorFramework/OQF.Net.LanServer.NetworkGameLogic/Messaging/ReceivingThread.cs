using System;
using Lib.Concurrency;
using NetMQ.Sockets;
using OQF.Net.LanMessaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.SendReceive;

namespace OQF.Net.LanServer.NetworkGameLogic.Messaging
{
	internal class ReceivingThread : IThread
	{
		public event Action<NetworkMessageBase> NewMessageAvailable;			

		private readonly Address serverAddress;
		private volatile bool stopRunning;
		
		public ReceivingThread(Address serverAddress)
		{
			this.serverAddress = serverAddress;
		}

		public void Run()
		{
			IsRunning = true;
			using (var socket = new PullSocket())
			{
				socket.Options.Linger = TimeSpan.Zero;
				socket.Bind(serverAddress.ZmqAddress + ":" + MessagingConstants.TcpIpPort.PushPullPort);

				while (!stopRunning)
				{
					var incommingMessage = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (incommingMessage == null)
						continue;					

					NewMessageAvailable?.Invoke(incommingMessage);
				}								
			}
			
			IsRunning = false;
		}

		public void Stop()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}