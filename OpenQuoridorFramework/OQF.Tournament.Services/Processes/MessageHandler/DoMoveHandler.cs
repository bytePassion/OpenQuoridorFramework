using System;
using OQF.Bot.Contracts.Moves;
using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;

namespace OQF.Tournament.Services.Processes.MessageHandler
{
	public class DoMoveHandler
    {
        private readonly IMessagingService messagingService;
        private readonly Action<Move> moveAvailableCallback;

        public DoMoveHandler(IMessagingService messagingService, Action<Move> moveAvailableCallback)
        {
            this.messagingService = messagingService;
            this.moveAvailableCallback = moveAvailableCallback;

            messagingService.NewMsgAvailable += MessagingServiceOnNewMsgAvailable;
        }

        private void MessagingServiceOnNewMsgAvailable(NetworkMessageBase msg)
        {
            if (msg.Type == NetworkMessageType.NextMoveResponse)
            {
                messagingService.NewMsgAvailable -= MessagingServiceOnNewMsgAvailable;
                var nextMoveMsg = (NextMoveResponse)msg;
                moveAvailableCallback?.Invoke(nextMoveMsg.NextMove);
            }
        }
    }
}