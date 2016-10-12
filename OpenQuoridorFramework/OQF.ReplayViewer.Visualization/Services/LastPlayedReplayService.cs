using System.IO;

namespace OQF.ReplayViewer.Visualization.Services
{
	public class LastPlayedReplayService : ILastPlayedReplayService
	{
		private const string FilePath = "LastReplay.qor";

		public string GetLastReplay()
		{
			if (!File.Exists(FilePath))
				return null;

			var botPath = File.ReadAllText(FilePath);

			return !string.IsNullOrWhiteSpace(botPath) 
				? botPath.Trim()
				: null;
		}
		
		public void SaveLastReplay(string botPath)
		{
			File.WriteAllText(FilePath, botPath);
		}
	}
}