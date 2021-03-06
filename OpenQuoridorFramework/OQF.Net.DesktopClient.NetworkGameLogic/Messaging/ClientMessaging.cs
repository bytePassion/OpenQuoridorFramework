﻿using System;
using System.Threading;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.NetworkGameLogic.Messaging
{
	public class ClientMessaging : DisposingObject, IClientMessaging
	{
		public event Action<NetworkMessageBase> NewIncomingMessage;

		private readonly SendingThread   sendingThread;
		private readonly ReceivingThread receivingThread;

		private readonly TimeoutBlockingQueue<NetworkMessageBase> outgoingMessageQueue;
		

		public ClientMessaging (Address serverAddress, ClientId newClientId)
		{
			outgoingMessageQueue = new TimeoutBlockingQueue<NetworkMessageBase>(500);
			sendingThread = new SendingThread(serverAddress, outgoingMessageQueue);

			receivingThread = new ReceivingThread(serverAddress, newClientId);
			receivingThread.NewMessageAvailable += OnNewMessageAvailable;

			new Thread(sendingThread.Run).Start();
			new Thread(receivingThread.Run).Start();
		}

		private void OnNewMessageAvailable (NetworkMessageBase incomingMessage)
		{
			NewIncomingMessage?.Invoke(incomingMessage);
		}


		public void SendMessage (NetworkMessageBase msg)
		{
			outgoingMessageQueue.Put(msg);
		}

		protected override void CleanUp ()
		{
			receivingThread.NewMessageAvailable -= OnNewMessageAvailable;

			sendingThread.Stop();
			receivingThread.Stop();
		}
	}
}