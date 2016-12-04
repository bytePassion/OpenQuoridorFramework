using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView
{
	internal class NetworkViewModelSampleData : INetworkViewModel
	{
		public NetworkViewModelSampleData()
		{
			ConnectionStatus = ConnectionStatus.Connected;
			GameStatus = GameStatus.NoGame; 

			ServerAddress = "10.10.10.10";			
			PlayerName = "xelor";
			NewGameName = "myGame01";

			AvailableOpenGames = new ObservableCollection<GameDisplayData>
			{
				new GameDisplayData(null, "game1", "p1"),
				new GameDisplayData(null, "game2", "p2"),
				new GameDisplayData(null, "game3", "p3"),
				new GameDisplayData(null, "game4", "p4"),
				new GameDisplayData(null, "game5", "p5")
			};

			SelectedOpenGame = AvailableOpenGames[2];

			ServerAddressPromt = "Server-address:";
			ServerAddressHint = "is-address";
			PlayerNamePromt = "playername:";
			PlayerNameHint = "playername";
			ConnectToServerButtonCaption = "connect";
			DisconnectFromServerButtonsCaptions = "disconnect";
			NewGameNamePromt = "new game name:";
			NewGameNameHint = "game-name";
			OpenGameListSectionHeader = "open game list";
			JoinGameButtonCaption = "join game";
			LeaveGameButtonCaption = "leave game";
			CancelCreatedGameButtonCaption = "cancel game";
			CreateGameButtonCaption = "create game";
		}

		public ConnectionStatus ConnectionStatus { get; }
		public GameStatus GameStatus { get; }

		public ICommand ConnectToServer      => null;
		public ICommand DisconnectFromServer => null;
		public ICommand CreateGame           => null;
		public ICommand CancelCreatedGame    => null;
		public ICommand JoinGame             => null;
		public ICommand LeaveGame            => null;

		public string NewGameName   { get; set; }
		public string ServerAddress { get; set; }
		public string PlayerName    { get; set; }		

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		public GameDisplayData SelectedOpenGame { get; set; }

		public string ServerAddressPromt                  { get; }
		public string ServerAddressHint                   { get; }
		public string PlayerNamePromt                     { get; }
		public string PlayerNameHint                      { get; }
		public string ConnectToServerButtonCaption        { get; }
		public string DisconnectFromServerButtonsCaptions { get; }
		public string NewGameNamePromt                    { get; }
		public string NewGameNameHint                     { get; }
		public string OpenGameListSectionHeader           { get; }
		public string JoinGameButtonCaption               { get; }
		public string CancelCreatedGameButtonCaption      { get; }
		public string LeaveGameButtonCaption              { get; }
		public string CreateGameButtonCaption             { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}