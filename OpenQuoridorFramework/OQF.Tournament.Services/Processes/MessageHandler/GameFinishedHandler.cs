using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;

namespace OQF.Tournament.Services.Processes.MessageHandler
{
	public class GameFinishedHandler
	{
		private readonly IMessagingService messagingService;

		public GameFinishedHandler(IMessagingService messagingService)
		{
			this.messagingService = messagingService;

			messagingService.NewMsgAvailable += MessagingServiceOnNewMsgAvailable;
		}

		private void MessagingServiceOnNewMsgAvailable(NetworkMessageBase msg)
		{
			if (msg.Type == NetworkMessageType.GameFinishedResponse)
			{				
				messagingService.NewMsgAvailable -= MessagingServiceOnNewMsgAvailable;
				messagingService.Dispose();
			}
		}
	}
}