using System;
using System.Threading;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;

namespace OQF.Tournament.Services.Messaging
{
	public class MessagingService : DisposingObject, IMessagingService
    {

        private readonly Communicator communicator;
        private readonly TimeoutBlockingQueue<NetworkMessageBase> queue; 

        public event Action<NetworkMessageBase> NewMsgAvailable;

        public MessagingService(string port)
        {
            queue = new TimeoutBlockingQueue<NetworkMessageBase>(1000);
            communicator = new Communicator(queue, port);

            communicator.NewMessageReceived += OnMessageReceived;

            new Thread(communicator.Run).Start();
        }

        private void OnMessageReceived(NetworkMessageBase msg)
        {
            if (msg.Type == NetworkMessageType.GameFinishedResponse)
            {
                communicator.Stop();
            }
            NewMsgAvailable?.Invoke(msg);
        }

        public void SendMessage(NetworkMessageBase msg)
        {
            queue.Put(msg);
        }

        protected override void CleanUp()
        {

            //communicator.Stop();

            queue.Dispose();

            communicator.NewMessageReceived -= OnMessageReceived;
        }
    }
}