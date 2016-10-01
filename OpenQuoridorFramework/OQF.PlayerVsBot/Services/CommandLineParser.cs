using System;

namespace OQF.PlayerVsBot.Services
{
	internal static class CommandLine
	{
		private const string DisableClosingDialogs = "-disableClosingDialogs";
		private const string DisableBotTimeOut     = "-disableBotTimeout";
		private const string BotDllPath            = "-botDll=";

		public static CommandLineArguments Parse(string[] args)
		{
			var disableClosingDialogs = false;
			var disableBotTimeout = false;
			var botDllPath = string.Empty;

			for (int i = 0; i < args.Length; i++)
			{
				if (string.Equals(args[i], DisableClosingDialogs, StringComparison.OrdinalIgnoreCase))
				{
					disableClosingDialogs = true;
				}

				if (string.Equals(args[i], DisableBotTimeOut, StringComparison.OrdinalIgnoreCase))
				{
					disableBotTimeout = true;
				}

				if (args[i].ToLower().StartsWith(BotDllPath.ToLower()))
				{
					var botPath = args[i].Substring(8);

					if (botPath.StartsWith("\""))
					{
						botPath = botPath.Substring(1);
					}

					if (botPath.EndsWith("\""))
					{
						botPath = botPath.Substring(0, botPath.Length-2);
					}

					botDllPath = botPath;
				}
			}

			return new CommandLineArguments(botDllPath, 
											disableClosingDialogs, 
											disableBotTimeout);
		}
	}
}
