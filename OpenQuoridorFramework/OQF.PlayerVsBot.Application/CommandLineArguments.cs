namespace OQF.PlayerVsBot.Application
{
	internal class CommandLineArguments
	{
		public CommandLineArguments(string botPath, bool disableClosingDialogs, bool disableBotTimeout)
		{
			BotPath = botPath;
			DisableClosingDialogs = disableClosingDialogs;
			DisableBotTimeout = disableBotTimeout;
		}

		public string BotPath               { get; }
		public bool   DisableClosingDialogs { get; }
		public bool   DisableBotTimeout     { get; }
	}
}
