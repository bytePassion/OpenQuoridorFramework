using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.BoardViewModelBase;
using OQF.Visualization.ReplayViewer.ViewModels.MainWindow.Helper;

namespace OQF.Visualization.ReplayViewer.ViewModels.MainWindow
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
						
		ObservableCollection<ProgressRow> ProgressRows { get; }

		string ProgressFilePath { get; set; }		
	}
}
