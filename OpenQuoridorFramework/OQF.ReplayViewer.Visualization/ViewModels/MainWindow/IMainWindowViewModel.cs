using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow.Helper;

namespace OQF.ReplayViewer.Visualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel BoardViewModel { get; }
		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }
		
		ICommand LoadGame      { get; }
		ICommand BrowseFile    { get; }
		ICommand ShowAboutHelp { get; }
		ICommand NextMove      { get; }
		ICommand PreviousMove  { get; }

		int MoveIndex { get; set; }
		int MaxMoveIndex { get; }
										
		bool IsReplayLoaded { get; }
						
		ObservableCollection<ProgressRow> ProgressRows { get; }

		string LodingString { get; set; }	
		
		
		string BrowseFileButtonCaption    { get; }
		string InputPromtLabelCaption     { get; }
		string LoadAndStartButtonCaption  { get; }	
		string NextStepButtonCaption      { get; }
		string PrevStepButtonCaption      { get; }
		string ProgressSectionHeader      { get; }
		string ShowAboutHelpButtonCaption { get; }
	}
}
