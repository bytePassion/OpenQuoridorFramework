using System.Collections.ObjectModel;
using System.Windows.Input;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel BoardViewModel { get; }

		ICommand Start     { get; }
		ICommand Restart   { get; }
		ICommand Stop      { get; }
		ICommand AboutHelp { get; }

		ICommand ApplyMove { get; }
		ICommand BrowseDll { get; }

		ObservableCollection<string> DebugMessages { get; } 
		ObservableCollection<string> GameProgress  { get; }				

		bool IsGameRunning { get; }

		string TopPlayerName    { get; } 
		string BottomPlayerName { get; }		

		int TopPlayerWallCountLeft    { get; }
		int BottomPlayerWallCountLeft { get; }

		string MoveInput    { get; set; }
		string DllPathInput { get; set; }		
	}
}
