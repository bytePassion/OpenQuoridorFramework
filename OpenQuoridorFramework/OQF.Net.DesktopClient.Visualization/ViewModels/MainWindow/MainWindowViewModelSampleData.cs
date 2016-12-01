using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardPlacementViewModel = new BoardPlacementViewModelSampleData();
			BoardViewModel = new BoardViewModelSampleData();
			ProgressViewModel = new ProgressViewModelSampleData();
			ActionBarViewModel = new ActionBarViewModelSampleData();
			BoardHorizontalLabelingViewModel = new BoardLabelingViewModelSampleData();
			BoardVerticalLabelingViewModel = new BoardLabelingViewModelSampleData();
			LocalPlayerBarViewModel = new LocalPlayerBarViewModelSampleData();

			ServerAddress = "10.10.10.10";
			Response = "positive";
			PlayerName = "xelor";
			NewGameName = "myGame01";
			IsBoardRotated = false;

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

		public IBoardPlacementViewModel BoardPlacementViewModel { get; }
		public IBoardViewModel BoardViewModel { get; }
		public IProgressViewModel ProgressViewModel { get; }
		public IActionBarViewModel ActionBarViewModel { get; }
		public IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }
		public ILocalPlayerBarViewModel LocalPlayerBarViewModel { get; }

		public ICommand ConnectToServer => null;
		public ICommand CreateGame => null;
		public ICommand JoinGame => null;

		public string NewGameName { get; set; }
		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }
		public string Response { get; }
		public bool IsBoardRotated { get; }

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		public GameDisplayData SelectedOpenGame { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}