using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.BoardViewModel;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar;
using OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView;
using OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar;
using OQF.PlayerVsBot.Visualization.ViewModels.ProgressView;

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel             BoardViewModel             { get; }
		IBoardPlacementViewModel    BoardPlacementViewModel    { get; }		
		IActionBarViewModel         ActionBarViewModel         { get; }
		IBotStatusBarViewModel      BotStatusBarViewModel      { get; }
		IHumanPlayerBarViewModel    HumanPlayerBarViewModel    { get; }
		IProgressViewModel          ProgressViewModel          { get; }
		IDebugMessageViewModel      DebugMessageViewModel      { get; }			
					
		ICommand CloseWindow { get; }		
		
		bool IsProgressSectionExpanded { get; set; }
		bool IsDebugSectionExpanded    { get; set; }

		bool IsDisabledOverlayVisible      { get; }				
		bool PreventWindowClosingToAskUser { get; }
													
		string ProgressCaption { get; }		
		string DebugCaption    { get; }		
	}
}
