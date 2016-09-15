using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OQF.PlayerVsBot.ViewModels.Board;
using OQF.PlayerVsBot.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;
using OQF.Visualization.Resources;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel = new BoardViewModelSampleData();
			BoardPlacementViewModel = new BoardPlacementViewModelSampleData();

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

			AvailableCountryCodes = new ObservableCollection<string>
			{
				"de",
				"en"
			};

			SelectedCountryCode = AvailableCountryCodes.First();

			TopPlayerName    = "PlayerOben";
			TopPlayerRestTime = "36";		

			TopPlayerWallCountLeft    = 10;
			BottomPlayerWallCountLeft = 9;
			
			DllPathInput = "blubb.dll";			

			GameStatus = GameStatus.Active;

			IsAutoScrollDebugMsgActive = true;
			IsAutoScrollProgressActive = true;

			IsDisabledOverlayVisible = true;
		}

		public IBoardViewModel BoardViewModel { get; }
		public IBoardPlacementViewModel BoardPlacementViewModel { get; }

		public ICommand Start          => null;		
		public ICommand ShowAboutHelp  => null;
		public ICommand Capitulate     => null;		
		public ICommand BrowseDll      => null;

		public ObservableCollection<string> DebugMessages  { get; }
		public ObservableCollection<string> GameProgress   { get; }

		public ObservableCollection<string> AvailableCountryCodes { get; }
		public string SelectedCountryCode { get; set; }

		public bool IsAutoScrollProgressActive { get; set; }
		public bool IsAutoScrollDebugMsgActive { get; set; }

		public bool IsDisabledOverlayVisible { get; }

		public GameStatus GameStatus { get; }		

		public string TopPlayerName { get; }
		public string TopPlayerRestTime { get; }

		public int TopPlayerWallCountLeft   { get; }
		public int BottomPlayerWallCountLeft { get; }
		
		public string DllPathInput { get; set; }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}