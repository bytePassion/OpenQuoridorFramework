using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.BoardViewModel;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		IBoardViewModel BoardViewModel { get; }
		IBoardPlacementViewModel BoardPlacementViewModel { get; }
		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand Start                             { get; }	
		ICommand StartWithProgress                 { get; }	
		ICommand StartWithProgressFromFile         { get; }	
		ICommand StartWithProgressFromString       { get; }	
		ICommand ShowAboutHelp                     { get; }
		ICommand Capitulate                        { get; }		
		ICommand BrowseDll                         { get; }
		ICommand DumpDebugToFile                   { get; }
		ICommand DumpProgressToFile                { get; }
		ICommand CloseWindow                       { get; }
		ICommand CopyCompressedProgressToClipBoard { get; }

		bool IsStartWithProgressPopupVisible { get; set; }

		ObservableCollection<string> DebugMessages { get; } 
		ObservableCollection<string> GameProgress  { get; }
		
		string CompressedProgress { get; }						

		bool IsAutoScrollProgressActive { get; set; }
		bool IsAutoScrollDebugMsgActive { get; set; }

		bool IsProgressSectionExpanded { get; set; }
		bool IsDebugSectionExpanded    { get; set; }

		bool IsDisabledOverlayVisible { get; }

		GameStatus GameStatus { get; }

		string TopPlayerName    { get; } 	
		string TopPlayerRestTime { get; }		

		int TopPlayerWallCountLeft    { get; }
		int BottomPlayerWallCountLeft { get; }
		string MovesLeft { get; }

		string DllPathInput { get; set; }

		bool PreventWindowClosingToAskUser { get; }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		////////                                                                                                 ////////
		////////                                          Captions                                               ////////
		////////                                                                                                 ////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
		
		string BrowseForBotButtonToolTipCaption          { get; }
		string StartGameButtonToolTipCaption             { get; }
		string StartWithProgressGameButtonToolTipCaption { get; }
		string OpenInfoButtonToolTipCaption              { get; }
		string BotNameLabelCaption                       { get; }
		string MaximalThinkingTimeLabelCaption           { get; }
		string WallsLeftLabelCaption                     { get; }
		string ProgressCaption                           { get; }
		string CompressedProgressCaption                 { get; }
		string CopyToClipboardButtonToolTipCpation       { get; }
		string AutoScrollDownCheckBoxCaption             { get; }
		string DebugCaption                              { get; }
		string CapitulateButtonCaption                   { get; }
		string HeaderCaptionPlayer                       { get; }
		string DumpDebugToFileButtonCaption              { get; }
		string DumpProgressToFileButtonCaption           { get; }
		string MovesLeftLabelCaption                     { get; }
	}
}
