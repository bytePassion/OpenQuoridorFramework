using System;
using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;
using OQF.Tournament.Services.Messaging;

namespace OQF.Tournament.Services.Processes.MessageHandler
{
	public class LoadBotHandler
    {
        private readonly IMessagingService messagingService;
        private readonly Action loadingFinishedCallback;

        public LoadBotHandler(IMessagingService messagingService, Action loadingFinishedCallback)
        {
            this.messagingService = messagingService;
            this.loadingFinishedCallback = loadingFinishedCallback;

            messagingService.NewMsgAvailable += MessagingServiceOnNewMsgAvailable;
        }

        private void MessagingServiceOnNewMsgAvailable(NetworkMessageBase msg)
        {
            if (msg.Type == NetworkMessageType.LoadBotResponse)
            {
                messagingService.NewMsgAvailable -= MessagingServiceOnNewMsgAvailable;
                loadingFinishedCallback?.Invoke();
            }
        }
    }
}