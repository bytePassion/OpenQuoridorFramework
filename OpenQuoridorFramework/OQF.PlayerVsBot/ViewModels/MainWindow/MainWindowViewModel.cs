using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Utils;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
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
using OQF.Visualization.Resources;
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

			TopPlayerName = "- - - - -";

			BrowseDll = new Command(DoBrowseDll,
								    () => GameStatus != GameStatus.Active,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			Start = new Command(async () => await DoStart(),
								() => GameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));
			StartWithProgress = new Command(async () => await DoStartWithProgress(),
								() => GameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));
			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			ShowAboutHelp = new Command(DoShowAboutHelp);
			DumpDebugToFile = new Command(DoDumpDebugToFile);
			DumpProgressToFile = new Command(DoDumpProgressToFile);

			GameStatus = GameStatus.Unloaded;

			DllPathInput              = applicationSettingsRepository.LastUsedBotPath;
			IsDebugSectionExpanded    = applicationSettingsRepository.IsDebugSectionExpanded;
			IsProgressSectionExpanded = applicationSettingsRepository.IsProgressSecionExpanded;

			botCountDownTimer = new Timer(BotCountDownTimerOnTick, null,Timeout.Infinite, Timeout.Infinite);		
			StopTimer();	
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
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.PlayerVsBot.Info,
								   InfoPage.PlayerVsBotApplicationInfo,
								   InfoPage.QuoridorRules, 
								   InfoPage.QuoridorNotation,
								   InfoPage.HowToWriteABot,
								   InfoPage.About);
		}

		private void OnNewDebugMsgAvailable(string s)
		{
			DebugMessages.Add(s);
		}

		private void DoDumpProgressToFile ()
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
				Title = Captions.PvB_DumpProgressFileDialogTitle
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
			{
				if (result.Value)
				{
					var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState);														 
					File.WriteAllText(dialog.FileName, fileText);
				}
			}
		}

		private void DoDumpDebugToFile ()
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
				Title = Captions.PvB_DumpDebugFileDialogTitle
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
			{
				if (result.Value)
				{
					var sb = new StringBuilder();

					for (int index = 0; index < DebugMessages.Count; index++)
					{ 
						var debugMessage = DebugMessages[index];
						sb.Append(debugMessage);

						if (index != DebugMessages.Count-1)
							sb.Append(Environment.NewLine);
					}				

					File.WriteAllText(dialog.FileName, sb.ToString());
				}
			}
		}

		private async void ExecuteWinDialog(bool reportWinning, Player player, WinningReason winningReason, Move invalidMove)
	    {
		    var winningDialogViewModel = new WinningDialogViewModel(reportWinning, winningReason, invalidMove);

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
                    Title = Captions.PvB_SaveGameProgressFileDialogTitle
                };

                var result = dialog.ShowDialog();

                if (result.HasValue)
                {
                    if (result.Value)
                    {
                        var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState)
                                                         .AndAppendWinnerAndReason(player, winningReason, invalidMove);

                        File.WriteAllText(dialog.FileName, fileText);
                    }
                }
            }
            
			winningDialogViewModel.Dispose();
        }

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			StopTimer();

			var reportWinning = player.PlayerType == PlayerType.BottomPlayer;
						


			ExecuteWinDialog(reportWinning, player, winningReason, invalidMove);

			GameStatus = GameStatus.Finished;
		}

		

		private void OnNewBoardStateAvailable(BoardState boardState)
		{			
			((Command)Capitulate).RaiseCanExecuteChanged();

			if (boardState == null)
			{
				GameStatus = GameStatus.Unloaded;

				TopPlayerName = "- - - - -";
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

		public ICommand Start              { get; }
		public ICommand StartWithProgress  { get; }
		public ICommand ShowAboutHelp      { get; }
		public ICommand Capitulate         { get; }		
		public ICommand BrowseDll          { get; }
		public ICommand DumpDebugToFile    { get; }
		public ICommand DumpProgressToFile { get; }

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

		private async Task DoStart()
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
				await NotificationService.Show(Captions.PvB_ErrorMsg_NoDllPath, Captions.ND_OkButtonCaption);				
				return;
			}

			if (!File.Exists(DllPathInput))
			{				
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_FileDoesNotExist} [{DllPathInput}]", 
											   Captions.ND_OkButtonCaption);
				return;
			}

			Assembly dllToLoad;

			try
			{
				dllToLoad = Assembly.LoadFile(DllPathInput);
			}
			catch
			{
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_FileIsNoAssembly} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);				
				return;
			}

			var uninitializedBotAndBotName = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBotAndBotName == null)
			{
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]",
											   Captions.ND_OkButtonCaption);				
				return;
			}

			applicationSettingsRepository.LastUsedBotPath = DllPathInput;									
			gameService.CreateGame(uninitializedBotAndBotName.Item1, uninitializedBotAndBotName.Item2, new GameConstraints(TimeSpan.FromSeconds(60), 100));
			
			((Command)Capitulate).RaiseCanExecuteChanged();
		}

		private async Task DoStartWithProgress ()
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
				await NotificationService.Show(Captions.PvB_ErrorMsg_NoDllPath, Captions.ND_OkButtonCaption);
				return;
			}

			if (!File.Exists(DllPathInput))
			{
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_FileDoesNotExist} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			Assembly dllToLoad;

			try
			{
				dllToLoad = Assembly.LoadFile(DllPathInput);
			}
			catch
			{
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_FileIsNoAssembly} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			var uninitializedBotAndBotName = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBotAndBotName == null)
			{
				await NotificationService.Show($"{Captions.PvB_ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]",
											   Captions.ND_OkButtonCaption);
				return;
			}


			var dialog = new OpenFileDialog
			{
				Filter = "text-file|*.txt"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
				if (result.Value)   
				{
					var progressText = File.ReadAllText(dialog.FileName);
					applicationSettingsRepository.LastUsedBotPath = DllPathInput;
					gameService.CreateGame(uninitializedBotAndBotName.Item1, uninitializedBotAndBotName.Item2, new GameConstraints(TimeSpan.FromSeconds(60), 100), progressText);

					((Command)Capitulate).RaiseCanExecuteChanged();
				}			
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
		public string DumpDebugToFileButtonCaption     => Captions.PvB_DumpDebugToFileButtonCaption;
		public string DumpProgressToFileButtonCaption  => Captions.PvB_DumpProgressToFileButtonCaption;

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
										 nameof(HeaderCaptionPlayer),
										 nameof(DumpDebugToFileButtonCaption),
										 nameof(DumpProgressToFileButtonCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}