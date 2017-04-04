using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Board.ViewModels.BoardPlacement;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar;
using OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar;
using OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView;
using OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar;

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel             BoardViewModel                   { get; }
		IBoardPlacementViewModel    BoardPlacementViewModel          { get; }		
		IActionBarViewModel         ActionBarViewModel               { get; }
		IBotStatusBarViewModel      BotStatusBarViewModel            { get; }
		IHumanPlayerBarViewModel    HumanPlayerBarViewModel          { get; }
		IProgressViewModel          ProgressViewModel                { get; }
		IDebugMessageViewModel      DebugMessageViewModel            { get; }	
		IBoardLabelingViewModel     BoardHorizontalLabelingViewModel { get; }
		IBoardLabelingViewModel     BoardVerticalLabelingViewModel   { get; }
					
		ICommand CloseWindow { get; }		
		
		bool IsProgressSectionExpanded { get; set; }
		bool IsDebugSectionExpanded    { get; set; }

		bool IsDisabledOverlayVisible      { get; }				
		bool PreventWindowClosingToAskUser { get; }
						
		bool IsBoardRotated { get; }
									
		string ProgressCaption { get; }		
		string DebugCaption    { get; }		
	}
}
