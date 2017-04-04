using System;
using System.Threading;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;

namespace OQF.Net.LanServer.NetworkGameLogic.Messaging
{
	public class ServerMessaging : DisposingObject, IServerMessaging
	{		
		public event Action<NetworkMessageBase> NewIncomingMessage;		

		private readonly SendingThread   sendingThread;
		private readonly ReceivingThread receivingThread;

		private readonly TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue;

		public ServerMessaging(Address serverAddress)
		{
			outgoingMessageQueue = new TimeoutBlockingQueue<NetworkMessageBase>(500);
			sendingThread = new SendingThread(serverAddress, outgoingMessageQueue);
			
			receivingThread = new ReceivingThread(serverAddress);
			receivingThread.NewMessageAvailable += OnNewMessageAvailable;

			new Thread(sendingThread.Run).Start();
			new Thread(receivingThread.Run).Start();
		}
		
		private void OnNewMessageAvailable(NetworkMessageBase incomingMessage)
		{			
			NewIncomingMessage?.Invoke(incomingMessage);
		}

		
		public void SendMessage(NetworkMessageBase msg)
		{ 			
			outgoingMessageQueue.Put(msg);
		}

		protected override void CleanUp()
		{			
			receivingThread.NewMessageAvailable -= OnNewMessageAvailable;

			sendingThread.Stop();
			receivingThread.Stop();
		}
	}
}