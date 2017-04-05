using System;
using bytePassion.Lib.ConcurrencyLib;
using NetMQ;
using NetMQ.Sockets;
using OQF.Tournament.Communication.Messages;

namespace OQF.Tournament.Services.Messaging
{
	public class Communicator : IThread
    {
        private readonly TimeoutBlockingQueue<NetworkMessageBase> outgoingMsgQueue;
        private readonly string port;
        private volatile bool stopRunning;

        public event Action<NetworkMessageBase> NewMessageReceived;
        public Communicator(TimeoutBlockingQueue<NetworkMessageBase> outgoingMsgQueue, string port)
        {
            this.outgoingMsgQueue = outgoingMsgQueue;
            this.port = port;
            stopRunning = false;
        }

        public void Run()
        {
            IsRunning = true;

            using (var socket = new RequestSocket($"tcp://localhost:{port}"))
            {
                while (!stopRunning)
                {
                    NetworkMessageBase outgoingMsg = null;
                    while (outgoingMsg == null && !stopRunning)
                    {
                        outgoingMsg = outgoingMsgQueue.TimeoutTake();
                    }
                    if(stopRunning) break;
                    socket.SendFrame(NetworkMessageCoding.Encode(outgoingMsg));

                    NetworkMessageBase incomingMsg = null;
                    var responseString = socket.ReceiveFrameString();
                    incomingMsg = NetworkMessageCoding.Decode(responseString);


                    if (incomingMsg != null)
                    {
                        NewMessageReceived?.Invoke(incomingMsg);
                    }
                }
                socket.Close();
            }
 
            IsRunning = false;
        }

        public void Stop()
        {
            stopRunning = true;
        }

        public bool IsRunning { get ;  private set; }
    }
}