namespace OQF.PlayerVsBot.Application
{
	internal class CommandLineArguments
	{
		public CommandLineArguments(string botPath, string progressFilePath, string progressString,
									bool disableClosingDialogs, bool disableBotTimeout)
		{
			BotPath = botPath;
			DisableClosingDialogs = disableClosingDialogs;
			DisableBotTimeout = disableBotTimeout;
			ProgressString = progressString;
			ProgressFilePath = progressFilePath;
		}
		
		public string BotPath               { get; }
		public string ProgressFilePath      { get; }
		public string ProgressString        { get; }
		public bool   DisableClosingDialogs { get; }
		public bool   DisableBotTimeout     { get; }		
	}
}
