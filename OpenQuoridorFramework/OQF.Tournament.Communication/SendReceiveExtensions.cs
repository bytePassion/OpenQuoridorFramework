using System;
using System.Text;
using NetMQ;
using OQF.Tournament.Communication.Messages;

namespace OQF.Tournament.Communication
{
    public static class SendReceiveExtensions
    {
        public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1.0);

        private static readonly Encoding Encoding = new UTF8Encoding();

        public static bool SendNetworkMsg(this NetMQSocket socket,
                                          NetworkMessageBase message,
                                          uint timeoutMilliSeconds = MessagingConstants.Timeout.StandardSendingTimeout)
        {
            var encodedMessage = NetworkMessageCoding.Encode(message);

            var outMsg = new Msg();
            outMsg.InitPool(Encoding.GetByteCount(encodedMessage));
            Encoding.GetBytes(encodedMessage, 0, encodedMessage.Length, outMsg.Data, 0);

            bool sendSuccessful;

            try
            {
                sendSuccessful = socket.TrySend(ref outMsg, TimeSpan.FromMilliseconds(timeoutMilliSeconds), false);
            }
            catch (Exception)
            {
                sendSuccessful = false;
            }

            outMsg.Close();
            return sendSuccessful;
        }

        public static NetworkMessageBase ReceiveNetworkMsg(this NetMQSocket socket, TimeSpan timeout)
        {
            var inMsg = new Msg();
            inMsg.InitEmpty();

            try
            {
                socket.TryReceive(ref inMsg, timeout);
            }
            catch (Exception)
            {
                return null;
            }

            var str = inMsg.Size > 0
                ? Encoding.GetString(inMsg.Data, 0, inMsg.Size)
                : string.Empty;

            inMsg.Close();

            return NetworkMessageCoding.Decode(str);
        }
    }

    public static class MessagingConstants
    {
        public static class Timeout
        {
            public const uint StandardSendingTimeout = 2000;
        }
    }
}
