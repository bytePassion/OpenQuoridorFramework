using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.PlayerVsBot.ViewModels.Board;
using OQF.PlayerVsBot.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardViewModel = new BoardViewModelSampleData();
			BoardPlacementViewModel = new BoardPlacementViewModelSampleData();
			LanguageSelectionViewModel = new LanguageSelectionViewModelSampleData();

			DebugMessages = new ObservableCollection<string>
			{
				"blubb1",
				"blubb2",
				"blubb3",
				"blubb4",
				"blubb5"
			};

			GameProgress = new ObservableCollection<string>
			{
				"1. e2 e8",
				"2. e3 e7"
			};			
			
			TopPlayerName    = "PlayerOben";
			TopPlayerRestTime = "36";		

			TopPlayerWallCountLeft    = 10;
			BottomPlayerWallCountLeft = 9;
			
			DllPathInput = "blubb.dll";			

			GameStatus = GameStatus.Active;

			IsAutoScrollDebugMsgActive = true;
			IsAutoScrollProgressActive = true;

			IsProgressSectionExpanded = true;
			IsDebugSectionExpanded    = false;

			IsDisabledOverlayVisible = true;

			BrowseForBotButtonToolTipCaption          = "bot dll laden";
			StartGameButtonToolTipCaption             = "start";
			StartWithProgressGameButtonToolTipCaption = "start";
			OpenInfoButtonToolTipCaption              = "Info";
			BotNameLabelCaption                       = "BotName";
			MaximalThinkingTimeLabelCaption           = "max. Rechenzeit";
			WallsLeftLabelCaption                     = "Walls";
			ProgressCaption                           = "Spielverlauf";
			AutoScrollDownCheckBoxCaption             = "Automatisch scrollen";
			DebugCaption                              = "Debug";
			CapitulateButtonCaption                   = "Kapitulieren";
			HeaderCaptionPlayer                       = "Spieler";
			DumpDebugToFileButtonCaption              = "Speichern";
			DumpProgressToFileButtonCaption           = "Speichern";
		}

		public IBoardViewModel BoardViewModel { get; }
		public IBoardPlacementViewModel BoardPlacementViewModel { get; }
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start              => null;
		public ICommand StartWithProgress  => null;
		public ICommand ShowAboutHelp      => null;
		public ICommand Capitulate         => null;		
		public ICommand BrowseDll          => null;
		public ICommand DumpDebugToFile    => null;
		public ICommand DumpProgressToFile => null;

		public ObservableCollection<string> DebugMessages  { get; }
		public ObservableCollection<string> GameProgress   { get; }		

		public bool IsAutoScrollProgressActive { get; set; }
		public bool IsAutoScrollDebugMsgActive { get; set; }

		public bool IsProgressSectionExpanded { get; set; }
		public bool IsDebugSectionExpanded    { get; set; }

		public bool IsDisabledOverlayVisible { get; }

		public GameStatus GameStatus { get; }		

		public string TopPlayerName { get; }
		public string TopPlayerRestTime { get; }

		public int TopPlayerWallCountLeft   { get; }
		public int BottomPlayerWallCountLeft { get; }
		
		public string DllPathInput { get; set; }



		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		////////                                                                                                 ////////
		////////                                          Captions                                               ////////
		////////                                                                                                 ////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////	

		public string BrowseForBotButtonToolTipCaption          { get; }
		public string StartGameButtonToolTipCaption             { get; }
		public string StartWithProgressGameButtonToolTipCaption { get; }
		public string OpenInfoButtonToolTipCaption              { get; }
		public string BotNameLabelCaption                       { get; }
		public string MaximalThinkingTimeLabelCaption           { get; }
		public string WallsLeftLabelCaption                     { get; }
		public string ProgressCaption                           { get; }
		public string AutoScrollDownCheckBoxCaption             { get; }
		public string DebugCaption                              { get; }
		public string CapitulateButtonCaption                   { get; }
		public string HeaderCaptionPlayer                       { get; }
		public string DumpDebugToFileButtonCaption              { get; }
		public string DumpProgressToFileButtonCaption           { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}