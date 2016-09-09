using System.Collections.ObjectModel;
using System.Windows.Input;
using OQF.SingleGameVisualization.ViewModels.Board;
using OQF.SingleGameVisualization.ViewModels.BoardPlacement;
using OQF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using WpfLib.ViewModelBase;

namespace OQF.SingleGameVisualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel BoardViewModel { get; }
		IBoardPlacementViewModel BoardPlacementViewModel { get; }

		ICommand Start         { get; }		
		ICommand ShowAboutHelp { get; }
		ICommand Capitulate    { get; }		
		ICommand BrowseDll     { get; }

		ObservableCollection<string> DebugMessages { get; } 
		ObservableCollection<string> GameProgress  { get; }				

		bool IsAutoScrollProgressActive { get; set; }
		bool IsAutoScrollDebugMsgActive { get; set; }

		bool IsDisabledOverlayVisible { get; }

		GameStatus GameStatus { get; }

		string TopPlayerName    { get; } 	
		string TopPlayerRestTime { get; }		

		int TopPlayerWallCountLeft    { get; }
		int BottomPlayerWallCountLeft { get; }
		
		string DllPathInput { get; set; }		
	}
}
