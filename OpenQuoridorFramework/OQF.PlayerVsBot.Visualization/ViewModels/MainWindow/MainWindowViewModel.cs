using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using Microsoft.Win32;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.AnalysisAndProgress.ProgressUtils.Validation;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.BoardViewModelBase;
using OQF.CommonUiElements.Dialogs.Notification;
using OQF.CommonUiElements.Dialogs.StringInput;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.CommonUiElements.Info;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.Global;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.Visualization.ViewModels.MainWindow.Helper;
using OQF.Resources;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		//private QProgress currentProgress;

		private readonly Timer botCountDownTimer;
		private DateTime startTime;

		private readonly IGameService gameService;
		private readonly IApplicationSettingsRepository applicationSettingsRepository;		
		private readonly bool disableClosingDialog;

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
		private string movesLeft;
		private string compressedProgress;
		private bool isStartWithProgressPopupVisible;


		public MainWindowViewModel (IBoardViewModel boardViewModel, 
									IBoardPlacementViewModel boardPlacementViewModel,
									ILanguageSelectionViewModel languageSelectionViewModel,
									IGameService gameService, 
									IApplicationSettingsRepository applicationSettingsRepository,									
									bool disableClosingDialog)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;
			this.applicationSettingsRepository = applicationSettingsRepository;			
			this.disableClosingDialog = disableClosingDialog;

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

			PreventWindowClosingToAskUser = !disableClosingDialog;

			TopPlayerName = "- - - - -";
			MovesLeft = "--";

			BrowseDll = new Command(DoBrowseDll,
								    () => GameStatus != GameStatus.Active,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			Start = new Command(async () => await DoStart(),
								() => GameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));

			StartWithProgress = new Command(() => { IsStartWithProgressPopupVisible = true; }, 
											() => GameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
											new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));

			StartWithProgressFromFile = new ParameterrizedCommand<string>(async filePath => { await DoStartWithProgressFromFile(filePath);
																							  IsStartWithProgressPopupVisible = false; });

			StartWithProgressFromString = new ParameterrizedCommand<string>(async progressString => { await DoStartWithProgressFromString(progressString);
																								      IsStartWithProgressPopupVisible = false; });

			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			CopyCompressedProgressToClipBoard = new Command(DoCopyCompressedProgressToClipBoard,
															() => !string.IsNullOrWhiteSpace(CompressedProgress),
															new PropertyChangedCommandUpdater(this, nameof(CompressedProgress)));

			ShowAboutHelp      = new Command(DoShowAboutHelp);
			DumpDebugToFile    = new Command(DoDumpDebugToFile);
			DumpProgressToFile = new Command(DoDumpProgressToFile);
			CloseWindow        = new Command(DoCloseWindow);

			GameStatus = GameStatus.Unloaded;

			DllPathInput              = applicationSettingsRepository.LastUsedBotPath;
			IsDebugSectionExpanded    = applicationSettingsRepository.IsDebugSectionExpanded;
			IsProgressSectionExpanded = applicationSettingsRepository.IsProgressSecionExpanded;

			botCountDownTimer = new Timer(BotCountDownTimerOnTick, null,Timeout.Infinite, Timeout.Infinite);		
			StopTimer();	
		}

		private void DoCopyCompressedProgressToClipBoard()
		{
			Clipboard.SetText(CompressedProgress);
		}

		private void DoCloseWindow()
		{
			PreventWindowClosingToAskUser = false;
			Application.Current.Windows
							   .OfType<Windows.MainWindow>()
							   .FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
							   ?.Close();
		}

		private void BotCountDownTimerOnTick(object sender)
		{
			var timeDiff = DateTime.Now - startTime;

			Application.Current?.Dispatcher.Invoke(() =>
			{				
				TopPlayerRestTime = GeometryLibUtils.DoubleFormat(Constants.GameConstraint.BotThinkingTimeSeconds - timeDiff.TotalSeconds, 2);
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
			Application.Current.Dispatcher.Invoke(() =>
			{
				DebugMessages.Add(s);
			});			
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

		private static string GetWinningOrLoosingMessage(bool reportWinning, WinningReason winningReason, Move invalidMove)
		{
			var sb = new StringBuilder();

			sb.Append(reportWinning
							? $"{Captions.WD_WinningMessage}"
							: $"{Captions.WD_LoosingMessage}");

			sb.Append($"\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");

			if (winningReason == WinningReason.InvalidMove)
				sb.Append($" [{invalidMove}]");

			sb.Append($"\n\n{Captions.WD_SaveGameRequest}");

			return sb.ToString();
		}

		private static string WinningReasonToString(WinningReason winningReason)
	    {
		    switch (winningReason)
		    {
			    case WinningReason.Capitulation:            return Captions.WinningReason_Capitulation;				
				case WinningReason.InvalidMove:             return Captions.WinningReason_InvalidMode;
				case WinningReason.ExceedanceOfMaxMoves:    return Captions.WinningReason_ExceedanceOfMaxMoves;
				case WinningReason.ExceedanceOfThoughtTime: return Captions.WinningReason_ExceedanceOfThoughtTime;
				case WinningReason.RegularQuoridorWin:      return Captions.WinningReason_RegularQuoridorWin;			    
		    }

		    return "";
	    }

		private async void ExecuteWinDialog(bool reportWinning, Player player, WinningReason winningReason, Move invalidMove)
		{
			var dialogResult = await YesNoDialogService.Show(GetWinningOrLoosingMessage(reportWinning, winningReason, invalidMove));
			 
            if (dialogResult)
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
        }

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				StopTimer();

				var reportWinning = player.PlayerType == PlayerType.BottomPlayer;						
				ExecuteWinDialog(reportWinning, player, winningReason, invalidMove);

				GameStatus = GameStatus.Finished;
			});			
		}

		

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				((Command)Capitulate).RaiseCanExecuteChanged();

				if (boardState == null)
				{
					GameStatus = GameStatus.Unloaded;

					TopPlayerName = "- - - - -";
					TopPlayerWallCountLeft = 10;
					BottomPlayerWallCountLeft = 10;
					MovesLeft = "--";
					TopPlayerRestTime = "--";

					GameProgress.Clear();
					CompressedProgress = "";
				}
				else
				{
					GameStatus = GameStatus.Active;

					TopPlayerName = boardState.TopPlayer.Player.Name;

					TopPlayerWallCountLeft    = boardState.TopPlayer.WallsToPlace;
					BottomPlayerWallCountLeft = boardState.BottomPlayer.WallsToPlace;

					if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
					{
						if (GameProgress.Count > 0)
						{
							GameProgress[GameProgress.Count - 1] = GameProgress[GameProgress.Count - 1] + $" {boardState.LastMove}";

							// TODO improve
							CompressedProgress = CreateQProgress.FromBoardState(boardState).Compressed;						
						}

						var currentMovesLeft = int.Parse(MovesLeft);
						MovesLeft = (currentMovesLeft - 1).ToString();

						StopTimer();
					}
					else
					{
						GameProgress.Add($"{GameProgress.Count + 1}: " + $"{boardState.LastMove}");

						// TODO improve
						CompressedProgress = CreateQProgress.FromBoardState(boardState).Compressed;

						StartTimer();
					}
				}			
			});			
		}		

		private void StartTimer()
		{
			startTime = DateTime.Now;
			TopPlayerRestTime = Constants.GameConstraint.BotThinkingTimeSeconds.ToString();
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

		public ICommand Start                             { get; }
		public ICommand StartWithProgress                 { get; }
		public ICommand StartWithProgressFromFile         { get; }
		public ICommand StartWithProgressFromString       { get; }
		public ICommand ShowAboutHelp                     { get; }
		public ICommand Capitulate                        { get; }		
		public ICommand BrowseDll                         { get; }
		public ICommand DumpDebugToFile                   { get; }
		public ICommand DumpProgressToFile                { get; }
		public ICommand CloseWindow                       { get; }
		public ICommand CopyCompressedProgressToClipBoard { get; }

		public bool IsStartWithProgressPopupVisible
		{
			get { return isStartWithProgressPopupVisible; }
			set { PropertyChanged.ChangeAndNotify(this, ref isStartWithProgressPopupVisible, value); }
		}

		public ObservableCollection<string> DebugMessages { get; }
		public ObservableCollection<string> GameProgress  { get; }

		public string CompressedProgress
		{
			get { return compressedProgress; }
			private set { PropertyChanged.ChangeAndNotify(this, ref compressedProgress, value);}
		}


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
				IsDisabledOverlayVisible      = value != GameStatus.Active;
				PreventWindowClosingToAskUser = !disableClosingDialog && value == GameStatus.Active;

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

		public string MovesLeft
		{
			get { return movesLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref movesLeft, value); }
		}

		public string DllPathInput
		{
			get { return dllPathInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref dllPathInput, value); }
		}

		public bool PreventWindowClosingToAskUser { get; private set; }

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
				CompressedProgress = "";
				DebugMessages.Clear();
			}

			GameStatus = GameStatus.Unloaded;

			if (string.IsNullOrWhiteSpace(DllPathInput))
			{
				await NotificationDialogService.Show(Captions.PvB_ErrorMsg_NoDllPath, Captions.ND_OkButtonCaption);				
				return;
			}

			if (!File.Exists(DllPathInput))
			{				
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileDoesNotExist} [{DllPathInput}]", 
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
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileIsNoAssembly} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);				
				return;
			}

			var uninitializedBotAndBotName = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBotAndBotName == null)
			{
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]",
											   Captions.ND_OkButtonCaption);				
				return;
			}

			MovesLeft = (Constants.GameConstraint.MaximalMovesPerGame + 1).ToString();
			applicationSettingsRepository.LastUsedBotPath = DllPathInput;									
			gameService.CreateGame(uninitializedBotAndBotName.Item1, 
								   uninitializedBotAndBotName.Item2, 
								   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds), 
													   Constants.GameConstraint.MaximalMovesPerGame));
			
			((Command)Capitulate).RaiseCanExecuteChanged();
		}		

		private async Task DoStartWithProgressFromFile (string filePath)
		{
			if (GameStatus == GameStatus.Finished)
			{
				gameService.StopGame();

				GameProgress.Clear();
				CompressedProgress = "";
				DebugMessages.Clear();
			}

			GameStatus = GameStatus.Unloaded;

			if (string.IsNullOrWhiteSpace(DllPathInput))
			{
				await NotificationDialogService.Show(Captions.PvB_ErrorMsg_NoDllPath, Captions.ND_OkButtonCaption);
				return;
			}

			if (!File.Exists(DllPathInput))
			{
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileDoesNotExist} [{DllPathInput}]",
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
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileIsNoAssembly} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			var uninitializedBotAndBotName = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBotAndBotName == null)
			{
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			var progressFilePath = string.Empty;

			if (string.IsNullOrWhiteSpace(filePath))
			{
				var dialog = new OpenFileDialog
				{
					Filter = "text-file|*.txt"
				};

				var result = dialog.ShowDialog();

				if (result.HasValue && result.Value)
				{					
					progressFilePath = dialog.FileName;					
				}
				else
				{
					return;
				}
			}
			else
			{
				progressFilePath = filePath;
			}

			var progressText = File.ReadAllText(progressFilePath);
			var initialProgress = CreateQProgress.FromReadableProgressTextFile(progressText);

			var verificationResult = ProgressVerifier.Verify(initialProgress, 
														     Constants.GameConstraint.MaximalMovesPerGame,
															 true);

			switch (verificationResult)
			{
				case ProgressVerificationResult.EmptyOrInvalid:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_ProgressFileCannotBeLoaded} [{progressFilePath}]" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_EmptyOrInvalid}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsInvalidMove:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_ProgressFileCannotBeLoaded} [{progressFilePath}]" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsInvalidMove}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsTerminatedGame:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_ProgressFileCannotBeLoaded} [{progressFilePath}]" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsTerminatedGame}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsMoreMovesThanAllowed:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_ProgressFileCannotBeLoaded} [{progressFilePath}]" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsMoreMovesThanAllowed}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.Valid:
				{
					applicationSettingsRepository.LastUsedBotPath = DllPathInput;
					MovesLeft = (Constants.GameConstraint.MaximalMovesPerGame + 1).ToString();
					gameService.CreateGame(uninitializedBotAndBotName.Item1,
										   uninitializedBotAndBotName.Item2,
										   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
															   Constants.GameConstraint.MaximalMovesPerGame), 
										   initialProgress);

					((Command)Capitulate).RaiseCanExecuteChanged();
					return;
				}
			}
		}

		private async Task DoStartWithProgressFromString (string progressString)
		{
			if (GameStatus == GameStatus.Finished)
			{
				gameService.StopGame();

				GameProgress.Clear();
				CompressedProgress = "";
				DebugMessages.Clear();
			}

			GameStatus = GameStatus.Unloaded;

			if (string.IsNullOrWhiteSpace(DllPathInput))
			{
				await NotificationDialogService.Show(Captions.PvB_ErrorMsg_NoDllPath, Captions.ND_OkButtonCaption);
				return;
			}

			if (!File.Exists(DllPathInput))
			{
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileDoesNotExist} [{DllPathInput}]",
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
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_FileIsNoAssembly} [{DllPathInput}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			var uninitializedBotAndBotName = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBotAndBotName == null)
			{
				await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_BotCanNotBeLoadedFromAsembly} [{dllToLoad.FullName}]",
											   Captions.ND_OkButtonCaption);
				return;
			}

			string compressedProgressString;

			if (string.IsNullOrWhiteSpace(progressString))
			{
				var result = await StringInputDialogService.Show(Captions.PvB_ProgressInputDialogPromt);

				if (!string.IsNullOrWhiteSpace(result))
				{
					compressedProgressString = result;
				}
				else
				{
					return;
				}				
			}
			else
			{
				compressedProgressString = progressString;
			}

			var initialProgress = CreateQProgress.FromCompressedProgressString(compressedProgressString);

			var verificationResult = ProgressVerifier.Verify(initialProgress, 														 
															 Constants.GameConstraint.MaximalMovesPerGame,
															 true);

			switch (verificationResult)
			{
				case ProgressVerificationResult.EmptyOrInvalid:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_CompressedProgressCannotBeLoaded}" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_EmptyOrInvalid}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsInvalidMove:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_CompressedProgressCannotBeLoaded}" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsInvalidMove}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsTerminatedGame:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_CompressedProgressCannotBeLoaded}" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsTerminatedGame}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.ProgressContainsMoreMovesThanAllowed:
				{
					await NotificationDialogService.Show($"{Captions.PvB_ErrorMsg_CompressedProgressCannotBeLoaded}" +
												   $"\n\n{Captions.PvB_ErrorMsg_Reason}:" +
												   $"\n{Captions.PVR_ProgressContainsMoreMovesThanAllowed}",
												   Captions.ND_OkButtonCaption);
					return;
				}
				case ProgressVerificationResult.Valid:
				{
					applicationSettingsRepository.LastUsedBotPath = DllPathInput;
					MovesLeft = (Constants.GameConstraint.MaximalMovesPerGame + 1).ToString();
					gameService.CreateGame(uninitializedBotAndBotName.Item1,
										   uninitializedBotAndBotName.Item2,
										   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
															   Constants.GameConstraint.MaximalMovesPerGame),
										   initialProgress);

					((Command)Capitulate).RaiseCanExecuteChanged();
					return;
				}
			}
		}

		private bool IsMoveApplyable ()
		{
			if (GameStatus != GameStatus.Active)
				return false;

			return gameService.CurrentBoardState?.CurrentMover.PlayerType == PlayerType.BottomPlayer;
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
		
		public string BrowseForBotButtonToolTipCaption          => Captions.PvB_BrowseForBotButtonToolTipCaption;
		public string StartGameButtonToolTipCaption             => Captions.PvB_StartGameButtonToolTipCaption;
		public string StartWithProgressGameButtonToolTipCaption => Captions.PvB_StartWithProgressGameButtonToolTipCaption;
		public string OpenInfoButtonToolTipCaption              => Captions.PvB_OpenInfoButtonToolTipCaption;
		public string BotNameLabelCaption                       => Captions.PvB_BotNameLabelCaption;
		public string MaximalThinkingTimeLabelCaption           => Captions.PvB_MaximalThinkingTimeLabelCaption;
		public string WallsLeftLabelCaption                     => Captions.PvB_WallsLeftLabelCaption;
		public string ProgressCaption                           => Captions.PvB_ProgressCaption;
		public string CompressedProgressCaption                 => Captions.PvB_CompressedProgressCaption;
		public string CopyToClipboardButtonToolTipCpation       => Captions.PvB_CopyToClipboardButtonToolTipCpation;
		public string AutoScrollDownCheckBoxCaption             => Captions.PvB_AutoScrollDownCheckBoxCaption;
		public string DebugCaption                              => Captions.PvB_DebugCaption;
		public string CapitulateButtonCaption                   => Captions.PvB_CapitulateButtonCaption;
		public string HeaderCaptionPlayer                       => Captions.PvB_HeaderCaptionPlayer;
		public string DumpDebugToFileButtonCaption              => Captions.PvB_DumpDebugToFileButtonCaption;
		public string DumpProgressToFileButtonCaption           => Captions.PvB_DumpProgressToFileButtonCaption;
		public string MovesLeftLabelCaption                     => Captions.PvB_MovesLeftLabelCaption;

		private void RefreshCaptions()
		{
			PropertyChanged.Notify(this, nameof(BrowseForBotButtonToolTipCaption),
										 nameof(StartGameButtonToolTipCaption),
										 nameof(StartWithProgressGameButtonToolTipCaption),
										 nameof(OpenInfoButtonToolTipCaption),
										 nameof(BotNameLabelCaption),
										 nameof(MaximalThinkingTimeLabelCaption),
										 nameof(WallsLeftLabelCaption),
										 nameof(ProgressCaption),
										 nameof(AutoScrollDownCheckBoxCaption),
										 nameof(DebugCaption),
										 nameof(CompressedProgressCaption),
										 nameof(CopyToClipboardButtonToolTipCpation),
										 nameof(CapitulateButtonCaption),
										 nameof(HeaderCaptionPlayer),
										 nameof(DumpDebugToFileButtonCaption),
										 nameof(DumpProgressToFileButtonCaption),
										 nameof(MovesLeftLabelCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}