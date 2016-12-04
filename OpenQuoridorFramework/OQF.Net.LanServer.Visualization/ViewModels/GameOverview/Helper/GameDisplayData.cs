using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.GameOverview.Helper
{
	public class GameDisplayData : INotifyPropertyChanged
	{
		public GameDisplayData(string gameName, string initiatorName, string opponendName)
		{
			GameName = gameName;
			InitiatorName = $"[{initiatorName}]";
			OpponendName = string.IsNullOrWhiteSpace(opponendName) 
										? "[----]" :
										$"[{opponendName}]";
		}

		public string GameName { get; }
		public string InitiatorName { get; }
		public string OpponendName { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
