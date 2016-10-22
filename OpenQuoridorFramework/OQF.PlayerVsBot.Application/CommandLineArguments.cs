namespace OQF.PlayerVsBot.Application
{
	internal class CommandLineArguments
	{
		public CommandLineArguments(string botPath, string progressFilePath, 
									bool disableClosingDialogs, bool disableBotTimeout)
		{
			BotPath = botPath;
			DisableClosingDialogs = disableClosingDialogs;
			DisableBotTimeout = disableBotTimeout;
			ProgressFilePath = progressFilePath;
		}
		
		public string BotPath               { get; }
		public string ProgressFilePath      { get; }
		public bool   DisableClosingDialogs { get; }
		public bool   DisableBotTimeout     { get; }		
	}
}
