using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.ReplayViewer.ViewModels.Board;

namespace OQF.ReplayViewer.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel BoardViewModel { get; }
		
		ICommand LoadGame      { get; }
		ICommand BrowseFile    { get; }
		ICommand ShowAboutHelp { get; }
		ICommand NextMove      { get; }
		ICommand PreviousMove  { get; }

		int MoveIndex { get; set; }
		int MaxMoveIndex { get; }
										
		bool IsReplayLoaded { get; }
						
		string ProgressFilePath { get; set; }		
	}
}
