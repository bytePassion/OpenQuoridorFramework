using OQF.Bot.Contracts;

namespace OQF.Utils
{
	public class BotLoadingResult
	{
		public BotLoadingResult(IQuoridorBot uninitializedBot, string botName, bool wasLodingSuccessful, string errorMessage)
		{
			UninitializedBot = uninitializedBot;
			BotName = botName;
			WasLodingSuccessful = wasLodingSuccessful;
			ErrorMessage = errorMessage;
		}

		public IQuoridorBot UninitializedBot { get; }
		public string BotName { get; }
		public bool WasLodingSuccessful { get; }
		public string ErrorMessage { get; }
	}
}