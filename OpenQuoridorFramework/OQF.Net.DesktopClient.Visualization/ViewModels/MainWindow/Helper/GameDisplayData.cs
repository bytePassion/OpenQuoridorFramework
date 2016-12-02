using System.ComponentModel;
using OQF.Net.LanMessaging.Types;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper
{
	public class GameDisplayData : INotifyPropertyChanged
	{
		public GameDisplayData(NetworkGameId gameId, string gameName, string initiatorName)
		{
			GameId = gameId;
			GameName = gameName;
			InitiatorName = initiatorName;
		}

		public NetworkGameId GameId { get; }
		public string GameName { get; }
		public string InitiatorName { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
