using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			ServerAddress = "10.10.10.10";
			Response = "positive";
			PlayerName = "xelor";
			NewGameName = "myGame01";

			AvailableOpenGames = new ObservableCollection<GameDisplayData>
			{
				new GameDisplayData(null, "game1"),
				new GameDisplayData(null, "game2"),
				new GameDisplayData(null, "game3"),
				new GameDisplayData(null, "game4"),
				new GameDisplayData(null, "game5")
			};

			SelectedOpenGame = AvailableOpenGames[2];
		}

		public ICommand ConnectToServer => null;
		public ICommand CreateGame => null;
		public ICommand JoinGame => null;

		public string NewGameName { get; set; }
		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }
		public string Response { get; }
		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		public GameDisplayData SelectedOpenGame { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}