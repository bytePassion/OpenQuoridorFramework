using System.IO;

namespace QCF.SingleGameVisualization.Services
{
	internal class LastUsedBotService : ILastUsedBotService
	{
		private const string FilePath = "LastUsedBot.qor";

		public string GetLastUsedBot()
		{
			if (!File.Exists(FilePath))
				return null;

			var botPath = File.ReadAllText(FilePath);

			return !string.IsNullOrWhiteSpace(botPath) 
				? botPath.Trim()
				: null;
		}

		public void SaveLastUsedBot(string botPath)
		{
			File.WriteAllText(FilePath, botPath);
		}
	}
}