using System;

namespace OQF.PlayerVsBot.Application
{
	internal static class CommandLine
	{
		private const string DisableClosingDialogsParameter = "-disableClosingDialog";
		private const string DisableBotTimeOutParameter     = "-disableBotTimeout";
		private const string BotDllPathParameter            = "-botDll=";
		private const string ProgressFilePathParameter      = "-progressFile=";
		private const string ProgressStringParameter        = "-progressString=";

		public static CommandLineArguments Parse(string[] args)
		{
			var disableClosingDialogs = false;
			var disableBotTimeout     = false;
			var botDllPath            = string.Empty;
			var progressString        = string.Empty;
			var progressFilePath      = string.Empty;

			foreach (string arg in args)
			{
				if (string.Equals(arg, DisableClosingDialogsParameter, StringComparison.OrdinalIgnoreCase))
				{
					disableClosingDialogs = true;
				}

				if (string.Equals(arg, DisableBotTimeOutParameter, StringComparison.OrdinalIgnoreCase))
				{
					disableBotTimeout = true;
				}

				if (arg.ToLower().StartsWith(BotDllPathParameter.ToLower()))
				{
					botDllPath = arg.Substring(BotDllPathParameter.Length);
				}

				if (arg.ToLower().StartsWith(ProgressFilePathParameter.ToLower()))
				{
					progressFilePath = arg.Substring(ProgressFilePathParameter.Length);
				}

				if (arg.ToLower().StartsWith(ProgressStringParameter.ToLower()))
				{
					progressString = arg.Substring(ProgressStringParameter.Length);
				}
			}

			return new CommandLineArguments(botDllPath, 
											progressFilePath,
											progressString,
											disableClosingDialogs, 
											disableBotTimeout);
		}
	}
}
