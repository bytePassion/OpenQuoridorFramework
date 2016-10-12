using System;

namespace OQF.PlayerVsBot.Application
{
	internal static class CommandLine
	{
		private const string DisableClosingDialogs = "-disableClosingDialog";
		private const string DisableBotTimeOut     = "-disableBotTimeout";
		private const string BotDllPath            = "-botDll=";

		public static CommandLineArguments Parse(string[] args)
		{
			var disableClosingDialogs = false;
			var disableBotTimeout = false;
			var botDllPath = string.Empty;

			foreach (string arg in args)
			{
				if (string.Equals(arg, DisableClosingDialogs, StringComparison.OrdinalIgnoreCase))
				{
					disableClosingDialogs = true;
				}

				if (string.Equals(arg, DisableBotTimeOut, StringComparison.OrdinalIgnoreCase))
				{
					disableBotTimeout = true;
				}

				if (arg.ToLower().StartsWith(BotDllPath.ToLower()))
				{
					botDllPath = arg.Substring(8);
				}
			}

			return new CommandLineArguments(botDllPath, 
											disableClosingDialogs, 
											disableBotTimeout);
		}
	}
}
