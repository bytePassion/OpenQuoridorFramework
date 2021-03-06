using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Board.ViewModels.BoardPlacement;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar;
using OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar;
using OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView;
using OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel                   = new BoardViewModelSampleData();
			BoardPlacementViewModel          = new BoardPlacementViewModelSampleData();		
			ActionBarViewModel               = new ActionBarViewModelSampleData();
			BotStatusBarViewModel            = new BotStatusBarViewModelSampleData();
			HumanPlayerBarViewModel          = new HumanPlayerBarViewModelSampleData();
			ProgressViewModel                = new ProgressViewModelSampleData();
			DebugMessageViewModel            = new DebugMessageViewModelSampleData();
			BoardHorizontalLabelingViewModel = new BoardLabelingViewModelSampleData();
			BoardVerticalLabelingViewModel   = new BoardLabelingViewModelSampleData();
																					
			IsProgressSectionExpanded = true;
			IsDebugSectionExpanded    = false;

			IsDisabledOverlayVisible = true;
			PreventWindowClosingToAskUser = false;
			IsBoardRotated = false;

			ProgressCaption = "Spielverlauf";			
			DebugCaption    = "Debug";																		
		}

		public IBoardViewModel             BoardViewModel                   { get; }
		public IBoardPlacementViewModel    BoardPlacementViewModel          { get; }		
		public IActionBarViewModel         ActionBarViewModel               { get; }
		public IBotStatusBarViewModel      BotStatusBarViewModel            { get; }
		public IHumanPlayerBarViewModel    HumanPlayerBarViewModel          { get; }
		public IProgressViewModel          ProgressViewModel                { get; }
		public IDebugMessageViewModel      DebugMessageViewModel            { get; }
		public IBoardLabelingViewModel     BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel     BoardVerticalLabelingViewModel   { get; }

		public ICommand CloseWindow   => null;
				
		public bool IsProgressSectionExpanded { get; set; }
		public bool IsDebugSectionExpanded    { get; set; }

		public bool IsDisabledOverlayVisible      { get; }		
		public bool PreventWindowClosingToAskUser { get; }
		public bool IsBoardRotated { get; }

		public string ProgressCaption { get; }		
		public string DebugCaption    { get; }		
								
		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}