using System.ComponentModel;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper
{
	public class GameDisplayData : INotifyPropertyChanged
	{
		public GameDisplayData(NetworkGameId gameId, string gameName)
		{
			GameId = gameId;
			GameName = gameName;
		}

		public NetworkGameId GameId { get; }
		public string GameName { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
