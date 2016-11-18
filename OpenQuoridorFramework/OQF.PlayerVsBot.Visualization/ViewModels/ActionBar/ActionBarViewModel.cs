using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using Microsoft.Win32;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.AnalysisAndProgress.ProgressUtils.Validation;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.CommonUiElements.Dialogs.Notification;
using OQF.CommonUiElements.Dialogs.StringInput;
using OQF.CommonUiElements.Info;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.Global;
using OQF.Resources;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar
{
	public class ActionBarViewModel : ViewModel, IActionBarViewModel
	{
		private readonly IApplicationSettingsRepository applicationSettingsRepository;
		private readonly IGameService gameService;

		private string dllPathInput;
		private bool isStartWithProgressPopupVisible;
		private GameStatus gameStatus;
		private string topPlayerName;

		public ActionBarViewModel(IApplicationSettingsRepository applicationSettingsRepository, 
								  IGameService gameService, 
								  ILanguageSelectionViewModel languageSelectionViewModel)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.applicationSettingsRepository = applicationSettingsRepository;
			this.gameService = gameService;
			LanguageSelectionViewModel = languageSelectionViewModel;

			gameService.NewGameStatusAvailable += OnNewGameStatusAvailable;
			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;

			BrowseDll = new Command(DoBrowseDll,
								    () => gameService.CurrentGameStatus != GameStatus.Active,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			Start = new Command(async () => await DoStart(),
								() => gameService.CurrentGameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));

			StartWithProgress = new Command(() => { IsStartWithProgressPopupVisible = true; }, 
											() => gameService.CurrentGameStatus != GameStatus.Active && !string.IsNullOrWhiteSpace(DllPathInput),
											new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(DllPathInput)));

			StartWithProgressFromFile = new ParameterrizedCommand<string>(async filePath => { await DoStartWithProgressFromFile(filePath);
																							  IsStartWithProgressPopupVisible = false; });

			StartWithProgressFromString = new ParameterrizedCommand<string>(async progressString => { await DoStartWithProgressFromString(progressString);
																								      IsStartWithProgressPopupVisible = false; });

			ShowAboutHelp = new Command(DoShowAboutHelp);

			TopPlayerName = "- - - - -";
			DllPathInput = applicationSettingsRepository.LastUsedBotPath;
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (boardState != null)
					TopPlayerName = boardState.TopPlayer.Player.Name;
			});			
		}

		private void OnNewGameStatusAvailable(GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				GameStatus = newGameStatus;

				if (GameStatus == GameStatus.Unloaded)
					TopPlayerName = "- - - - -";
			});									
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start                             { get; }
		public ICommand StartWithProgress                 { get; }
		public ICommand StartWithProgressFromFile         { get; }
		public ICommand StartWithProgressFromString       { get; }
		public ICommand ShowAboutHelp                     { get; }
		public ICommand BrowseDll                         { get; }

		private GameStatus GameStatus
		{
			get { return gameStatus; }
			set { PropertyChanged.ChangeAndNotify(this, ref gameStatus, value); }
		}

		public string DllPathInput
		{
			get { return dllPathInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref dllPathInput, value); }
		}

		public bool IsStartWithProgressPopupVisible
		{
			get { return isStartWithProgressPopupVisible; }
			set { PropertyChanged.ChangeAndNotify(this, ref isStartWithProgressPopupVisible, value); }
		}

		public string TopPlayerName
		{
			get { return topPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerName, value); }
		}

		private void DoShowAboutHelp ()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.PlayerVsBot.Info,
								   InfoPage.PlayerVsBotApplicationInfo,
								   InfoPage.QuoridorRules,
								   InfoPage.QuoridorNotation,
								   InfoPage.HowToWriteABot,
								   InfoPage.About);
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
			if (gameService.CurrentGameStatus == GameStatus.Finished)
			{
				gameService.StopGame();				
			}
			

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
			
			applicationSettingsRepository.LastUsedBotPath = DllPathInput;									
			gameService.CreateGame(uninitializedBotAndBotName.Item1, 
								   uninitializedBotAndBotName.Item2, 
								   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds), 
													   Constants.GameConstraint.MaximalMovesPerGame));						
		}		

		private async Task DoStartWithProgressFromFile (string filePath)
		{
			if (gameService.CurrentGameStatus == GameStatus.Finished)
			{
				gameService.StopGame();				
			}			

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

			string progressFilePath;

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
					gameService.CreateGame(uninitializedBotAndBotName.Item1,
										   uninitializedBotAndBotName.Item2,
										   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
															   Constants.GameConstraint.MaximalMovesPerGame), 
										   initialProgress);
					
					return;
				}
			}
		}

		private async Task DoStartWithProgressFromString (string progressString)
		{
			if (gameService.CurrentGameStatus == GameStatus.Finished)
			{
				gameService.StopGame();				
			}			

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
					gameService.CreateGame(uninitializedBotAndBotName.Item1,
										   uninitializedBotAndBotName.Item2,
										   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
															   Constants.GameConstraint.MaximalMovesPerGame),
										   initialProgress);					
					return;
				}
			}
		}

		public string BrowseForBotButtonToolTipCaption          => Captions.PvB_BrowseForBotButtonToolTipCaption;
		public string StartGameButtonToolTipCaption             => Captions.PvB_StartGameButtonToolTipCaption;
		public string StartWithProgressGameButtonToolTipCaption => Captions.PvB_StartWithProgressGameButtonToolTipCaption;
		public string OpenInfoButtonToolTipCaption              => Captions.PvB_OpenInfoButtonToolTipCaption;
		public string HeaderCaptionPlayer                       => Captions.PvB_HeaderCaptionPlayer;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(BrowseForBotButtonToolTipCaption),
										 nameof(StartGameButtonToolTipCaption),
										 nameof(StartWithProgressGameButtonToolTipCaption),
										 nameof(HeaderCaptionPlayer),
										 nameof(OpenInfoButtonToolTipCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			((Command)BrowseDll).Dispose();
			((Command)Start).Dispose();
			((Command)StartWithProgress).Dispose();
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
