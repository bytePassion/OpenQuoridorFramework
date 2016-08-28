using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using QCF.GameEngine.Contracts.GameElements;
using QCF.UiTools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		ICommand Start     { get; }
		ICommand Restart   { get; }
		ICommand Stop      { get; }
		ICommand AboutHelp { get; }

		ICommand ApplyMove { get; }
		ICommand BrowseDll { get; }

		ObservableCollection<string> DebugMessages { get; } 
		ObservableCollection<string> GameProgress  { get; }
		
		ObservableCollection<Wall>        VisibleWalls   { get; } 
		ObservableCollection<PlayerState> VisiblePlayers { get; } 

		bool IsGameRunning { get; }

		string TopPlayerName    { get; } 
		string BottomPlayerName { get; }		

		int TopPlayerWallCountLeft    { get; }
		int BottomPlayerWallCountLeft { get; }

		string MoveInput    { get; set; }
		string DllPathInput { get; set; }

		Size BoardSize { get; set; }
	}
}
