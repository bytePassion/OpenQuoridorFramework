using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.SingleGameVisualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel = new BoardViewModelSampleData();

			DebugMessages = new ObservableCollection<string>
			{
				"blubb1",
				"blubb2",
				"blubb3",
				"blubb4",
				"blubb5"
			};

			GameProgress = new ObservableCollection<string>
			{
				"1. e2 e8",
				"2. e3 e7"
			};			

			TopPlayerName    = "PlayerOben";
			TopPlayerRestTime = "36";		

			TopPlayerWallCountLeft    = 10;
			BottomPlayerWallCountLeft = 9;

			MoveInput = "e8";
			DllPathInput = "blubb.dll";			

			GameStatus = GameStatus.Active;

			IsAutoScrollDebugMsgActive = true;
			IsAutoScrollProgressActive = true;
		}

		public IBoardViewModel BoardViewModel { get; }

		public ICommand Start          => null;
		public ICommand Restart        => null;
		public ICommand Stop           => null;
		public ICommand ShowAboutHelp  => null;
		public ICommand Capitulate     => null;
		public ICommand ApplyMove      => null;
		public ICommand BrowseDll      => null;

		public ObservableCollection<string> DebugMessages  { get; }
		public ObservableCollection<string> GameProgress   { get; }

		public bool IsAutoScrollProgressActive { get; set; }
		public bool IsAutoScrollDebugMsgActive { get; set; }

		public GameStatus GameStatus { get; }		

		public string TopPlayerName { get; }
		public string TopPlayerRestTime { get; }

		public int TopPlayerWallCountLeft   { get; }
		public int BottomPlayerWallCountLeft { get; }

		public string MoveInput { get; set; }
		public string DllPathInput { get; set; }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}