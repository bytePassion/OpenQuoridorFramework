using System;
using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;
using OQF.Tournament.Services.Messaging;

namespace OQF.Tournament.Services.Processes.MessageHandler
{
	public class InitBotHandler
    {
        private readonly IMessagingService messagingService;
        private readonly Action initFinishedCallback;

        public InitBotHandler(IMessagingService messagingService, Action initFinishedCallback)
        {
            this.messagingService = messagingService;
            this.initFinishedCallback = initFinishedCallback;

            messagingService.NewMsgAvailable += MessagingServiceOnNewMsgAvailable;
        }

        private void MessagingServiceOnNewMsgAvailable(NetworkMessageBase msg)
        {
            if (msg.Type == NetworkMessageType.InitGameResponse)
            {
                messagingService.NewMsgAvailable -= MessagingServiceOnNewMsgAvailable;
                initFinishedCallback?.Invoke();
            }
        }
    }
}