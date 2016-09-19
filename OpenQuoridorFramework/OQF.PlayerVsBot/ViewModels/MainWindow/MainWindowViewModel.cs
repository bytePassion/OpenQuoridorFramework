using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Utils;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;
using OQF.PlayerVsBot.Services;
using OQF.PlayerVsBot.Services.SettingsRepository;
using OQF.PlayerVsBot.ViewModels.Board;
using OQF.PlayerVsBot.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;
using OQF.PlayerVsBot.ViewModels.WinningDialog;
using OQF.Utils;
using OQF.Visualization.Common.Info;
using OQF.Visualization.Common.Language;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.PlayerVsBot.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly Timer botCountDownTimer;
		private DateTime startTime;

		private readonly IGameService gameService;
		private readonly IApplicationSettingsRepository applicationSettingsRepository;


		private string dllPathInput;		
		private int bottomPlayerWallCountLeft;
		private int topPlayerWallCountLeft;		
		private string topPlayerName;
		private GameStatus gameStatus;
		private bool isAutoScrollProgressActive;
		private bool isAutoScrollDebugMsgActive;
		private string topPlayerRestTime;
		private bool isDisabledOverlayVisible;
		private bool isProgressSectionExpanded;
		private bool isDebugSectionExpanded;


		public MainWindowViewModel (IBoardViewModel boardViewModel, 
									IBoardPlacementViewModel boardPlacementViewModel,
									ILanguageSelectionViewModel languageSelectionViewModel,
									IGameService gameService, 
									IApplicationSettingsRepository applicationSettingsRepository)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;
			this.applicationSettingsRepository = applicationSettingsRepository;			

			LanguageSelectionViewModel = languageSelectionViewModel;
			BoardPlacementViewModel = boardPlacementViewModel;
			BoardViewModel = boardViewModel;

			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();						

			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameService.WinnerAvailable        += OnWinnerAvailable;
			gameService.NewDebugMsgAvailable   += OnNewDebugMsgAvailable;

			IsAutoScrollDebugMsgActive = true;
			IsAutoScrollProgressActive = true;

			BrowseDll = new Command(DoBrowseDll,
								    () => GameStatus != GameStatus.Active,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			Start = new Command(DoStart,
								() => GameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));							
			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			ShowAboutHelp = new Command(DoShowAboutHelp);

			GameStatus = GameStatus.Unloaded;

			DllPathInput              = applicationSettingsRepository.LastUsedBotPath;
			IsDebugSectionExpanded    = applicationSettingsRepository.IsDebugSectionExpanded;
			IsProgressSectionExpanded = applicationSettingsRepository.IsProgressSecionExpanded;

			botCountDownTimer = new Timer(BotCountDownTimerOnTick, null,Timeout.Infinite, Timeout.Infinite);			
		}		

		private void BotCountDownTimerOnTick(object sender)
		{
			var timeDiff = DateTime.Now - startTime;

			Application.Current?.Dispatcher.Invoke(() =>
			{
				TopPlayerRestTime = GeometryLibUtils.DoubleFormat(60.0 - timeDiff.TotalSeconds, 2);
			});			
		}

		private void DoShowAboutHelp()
		{
			InfoWindowService.Show(InfoPage.PlayerVsBotApplicationInfo,
								   InfoPage.QuoridorRules, 
								   InfoPage.QuoridorNotation,
								   InfoPage.HowToWriteABot,
								   InfoPage.About);
		}

		private void OnNewDebugMsgAvailable(string s)
		{
			DebugMessages.Add(s);
		}

	    private async void ExecuteWinDialog(bool reportWinning, Player player, WinningReason winningReason)
	    {
		    var winningDialogViewModel = new WinningDialogViewModel(reportWinning, winningReason);

			var view = new Views.WinningDialog
	        {
                DataContext = winningDialogViewModel
	        };

	        var dialogResult = await DialogHost.Show(view, "RootDialog");

            if ((bool)dialogResult)
            {
                var dialog = new SaveFileDialog()
                {
                    Filter = "textFiles |*.txt",
                    AddExtension = true,
                    CheckFileExists = false,
                    OverwritePrompt = true,
                    ValidateNames = true,
                    CheckPathExists = true,
                    CreatePrompt = false,
                    Title = "Save Game Progress of currently ended game"
                };

                var result = dialog.ShowDialog();

                if (result.HasValue)
                {
                    if (result.Value)
                    {
                        var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState)
                                                         .AndAppendWinnerAndReason(player, winningReason);

                        File.WriteAllText(dialog.FileName, fileText);
                    }
                }
            }
            
			winningDialogViewModel.Dispose();
        }

		private void OnWinnerAvailable(Player player, WinningReason winningReason)
		{
			StopTimer();

			var reportWinning = player.PlayerType == PlayerType.BottomPlayer;
						


			ExecuteWinDialog(reportWinning, player, winningReason);

			GameStatus = GameStatus.Finished;
		}

		

		private void OnNewBoardStateAvailable(BoardState boardState)
		{			
			((Command)Capitulate).RaiseCanExecuteChanged();

			if (boardState == null)
			{
				GameStatus = GameStatus.Unloaded;

				TopPlayerName = string.Empty;
				TopPlayerWallCountLeft = 10;
				BottomPlayerWallCountLeft = 10;

				GameProgress.Clear();
			}
			else
			{
				GameStatus = GameStatus.Active;

				TopPlayerName = boardState.TopPlayer.Player.Name;

				TopPlayerWallCountLeft = boardState.TopPlayer.WallsToPlace;
				BottomPlayerWallCountLeft = boardState.BottomPlayer.WallsToPlace;

				if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
				{
					if (GameProgress.Count > 0)
						GameProgress[GameProgress.Count - 1] = GameProgress[GameProgress.Count - 1] + $" {boardState.LastMove}";

					StopTimer();
				}
				else
				{
					GameProgress.Add($"{GameProgress.Count+1}. {boardState.LastMove}");
					StartTimer();
				}
			}			
		}

		private void StartTimer()
		{
			startTime = DateTime.Now;
			TopPlayerRestTime = "60";
			botCountDownTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
		}

		private void StopTimer()
		{
			startTime = default(DateTime);
			botCountDownTimer.Change(Timeout.Infinite, Timeout.Infinite);
			TopPlayerRestTime = "--";
		}

		public IBoardViewModel BoardViewModel { get; }
		public IBoardPlacementViewModel BoardPlacementViewModel { get; }
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start         { get; }		
		public ICommand ShowAboutHelp { get; }
		public ICommand Capitulate    { get; }		
		public ICommand BrowseDll     { get; }

		public ObservableCollection<string> DebugMessages { get; }
		public ObservableCollection<string> GameProgress  { get; }

		
		public bool IsAutoScrollProgressActive
		{
			get { return isAutoScrollProgressActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollProgressActive, value); }
		}

		public bool IsAutoScrollDebugMsgActive
		{
			get { return isAutoScrollDebugMsgActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollDebugMsgActive, value); }
		}

		public bool IsProgressSectionExpanded
		{
			get { return isProgressSectionExpanded; }
			set
			{
				if (value != isProgressSectionExpanded)
				{
					applicationSettingsRepository.IsProgressSecionExpanded = value;
				}

				PropertyChanged.ChangeAndNotify(this, ref isProgressSectionExpanded, value);
			}
		}

		public bool IsDebugSectionExpanded
		{
			get { return isDebugSectionExpanded; }
			set
			{
				if (value != isDebugSectionExpanded)
				{
					applicationSettingsRepository.IsDebugSectionExpanded = value;
				}

				PropertyChanged.ChangeAndNotify(this, ref isDebugSectionExpanded, value);
			}
		}

		public bool IsDisabledOverlayVisible
		{
			get { return isDisabledOverlayVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isDisabledOverlayVisible, value); }
		}

		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set
			{
				IsDisabledOverlayVisible = value != GameStatus.Active;

				PropertyChanged.ChangeAndNotify(this, ref gameStatus, value);
			}
		}


		public string TopPlayerName
		{
			get { return topPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerName, value); }
		}

		public string TopPlayerRestTime
		{
			get { return topPlayerRestTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerRestTime, value); }
		}

		public int TopPlayerWallCountLeft
		{
			get { return topPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerWallCountLeft, value); }
		}

		public int BottomPlayerWallCountLeft
		{
			get { return bottomPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref bottomPlayerWallCountLeft, value); }
		}
		
		public string DllPathInput
		{
			get { return dllPathInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref dllPathInput, value); }
		}		

		private void DoBrowseDll()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "dll|*.dll"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
				if (result.Value)
					DllPathInput = dialog.FileName;
        }

		private void DoStart()
		{
			if (GameStatus == GameStatus.Finished)
			{
				gameService.StopGame();

				GameProgress.Clear();
				DebugMessages.Clear();
			}

			GameStatus = GameStatus.Unloaded;

			if (string.IsNullOrWhiteSpace(DllPathInput))
			{
				MessageBox.Show("bevor das Spiel gestartet werden kann muss eine bot-Dll ausgewählt werden");
				return;
			}

			if (!File.Exists(DllPathInput))
			{
				MessageBox.Show($"die datei {DllPathInput} existiert nicht");
				return;
			}

			Assembly dllToLoad;

			try
			{
				dllToLoad = Assembly.LoadFile(DllPathInput);
			}
			catch
			{
				MessageBox.Show($"die datei {DllPathInput} kann nicht als Assembly geladen werden");
				return;
			}

			var uninitializedBot = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBot == null)
			{
				MessageBox.Show($"die Assemply {dllToLoad.FullName} kann nicht als IQuoridorBot instantiiert werden");
				return;
			}

			applicationSettingsRepository.LastUsedBotPath = DllPathInput;									
			gameService.CreateGame(uninitializedBot, new GameConstraints(TimeSpan.FromSeconds(60), 100));
			
			((Command)Capitulate).RaiseCanExecuteChanged();
		}
				
		private bool IsMoveApplyable ()
		{
			if (GameStatus != GameStatus.Active)
				return false;

			return gameService.CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer;
		}
		

		private void DoCapitulate ()
		{
			gameService.ReportHumanMove(new Capitulation());
		}




		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		////////                                                                                                 ////////
		////////                                          Captions                                               ////////
		////////                                                                                                 ////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////	


		public string BrowseForBotButtonToolTipCaption => Captions.PvB_BrowseForBotButtonToolTipCaption;
		public string StartGameButtonToolTipCaption    => Captions.PvB_StartGameButtonToolTipCaption;
		public string OpenInfoButtonToolTipCaption     => Captions.PvB_OpenInfoButtonToolTipCaption;
		public string BotNameLabelCaption              => Captions.PvB_BotNameLabelCaption;
		public string MaximalThinkingTimeLabelCaption  => Captions.PvB_MaximalThinkingTimeLabelCaption;
		public string WallsLeftLabelCaption            => Captions.PvB_WallsLeftLabelCaption;
		public string ProgressCaption                  => Captions.PvB_ProgressCaption;
		public string AutoScrollDownCheckBoxCaption    => Captions.PvB_AutoScrollDownCheckBoxCaption;
		public string DebugCaption                     => Captions.PvB_DebugCaption;
		public string CapitulateButtonCaption          => Captions.PvB_CapitulateButtonCaption;
		public string HeaderCaptionPlayer              => Captions.PvB_HeaderCaptionPlayer;

		private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(BrowseForBotButtonToolTipCaption),
										 nameof(StartGameButtonToolTipCaption),
										 nameof(OpenInfoButtonToolTipCaption),
										 nameof(BotNameLabelCaption),
										 nameof(MaximalThinkingTimeLabelCaption),
										 nameof(WallsLeftLabelCaption),
										 nameof(ProgressCaption),
										 nameof(AutoScrollDownCheckBoxCaption),
										 nameof(DebugCaption),
										 nameof(CapitulateButtonCaption),
										 nameof(HeaderCaptionPlayer));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}