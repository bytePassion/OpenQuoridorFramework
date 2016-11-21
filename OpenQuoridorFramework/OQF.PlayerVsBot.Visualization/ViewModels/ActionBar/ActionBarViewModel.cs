using System;
using System.ComponentModel;
using System.IO;
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
		private bool isBotLoaded;

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
								() => gameService.CurrentGameStatus != GameStatus.Active && IsBotLoaded,
								new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(IsBotLoaded)));

			StartWithProgress = new Command(() => { IsStartWithProgressPopupVisible = true; }, 
											() => gameService.CurrentGameStatus != GameStatus.Active && IsBotLoaded,
											new PropertyChangedCommandUpdater(this, nameof(GameStatus), nameof(IsBotLoaded)));

			StartWithProgressFromFile = new ParameterrizedCommand<string>(async filePath => { await DoStartWithProgressFromFile(filePath);
																							  IsStartWithProgressPopupVisible = false; });

			StartWithProgressFromString = new ParameterrizedCommand<string>(async progressString => { await DoStartWithProgressFromString(progressString);
																								      IsStartWithProgressPopupVisible = false; });

			ShowAboutHelp = new Command(DoShowAboutHelp);			

			DllPathInput = applicationSettingsRepository.LastUsedBotPath;

			var loadingResult = BotLoader.GetUninitializedBot(DllPathInput);

			if (loadingResult.WasLodingSuccessful)
			{
				TopPlayerName = loadingResult.BotName;
				IsBotLoaded = true;				
			}
			else
			{
				TopPlayerName = Captions.PvB_NoBotLoadedCaption;
				IsBotLoaded = false;
			}
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
			});									
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start                             { get; }
		public ICommand StartWithProgress                 { get; }
		public ICommand StartWithProgressFromFile         { get; }
		public ICommand StartWithProgressFromString       { get; }
		public ICommand ShowAboutHelp                     { get; }
		public ICommand BrowseDll                         { get; }

		private bool IsBotLoaded
		{
			get { return isBotLoaded; }
			set { PropertyChanged.ChangeAndNotify(this, ref isBotLoaded, value); }
		}

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

		private async void DoBrowseDll()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "dll|*.dll"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue && result.Value)
			{
				DllPathInput = dialog.FileName;
				await TryToGetUninitializedBot();
			}				
		}

		private async Task DoStart()
		{						
			var uninitializedBotAndBotName = await TryToGetUninitializedBot();

			if (uninitializedBotAndBotName == null)
				return;
														
			gameService.CreateGame(uninitializedBotAndBotName.UninitializedBot, 
								   uninitializedBotAndBotName.BotName, 
								   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds), 
													   Constants.GameConstraint.MaximalMovesPerGame));						
		}

		private async Task<BotLoadingResult> TryToGetUninitializedBot()
		{
			var loadingResult = BotLoader.GetUninitializedBot(DllPathInput);

			if (loadingResult.WasLodingSuccessful)
			{
				TopPlayerName = loadingResult.BotName;
				IsBotLoaded = true;

				return loadingResult;
			}
			else
			{
				TopPlayerName = Captions.PvB_NoBotLoadedCaption;
				IsBotLoaded = false;

				await NotificationDialogService.Show(loadingResult.ErrorMessage, 
													 Captions.ND_OkButtonCaption);

				return null;
			}
		}

		private async Task DoStartWithProgressFromFile (string filePath)
		{
			var loadingResult = await TryToGetUninitializedBot();

			if (loadingResult == null)						
				return;
			
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

			if (verificationResult.Result == VerificationResult.Valid)
			{
				applicationSettingsRepository.LastUsedBotPath = DllPathInput;
				gameService.CreateGame(loadingResult.UninitializedBot,
									   loadingResult.BotName,
					                   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
						                                   Constants.GameConstraint.MaximalMovesPerGame),
					                   initialProgress);
			}
			else
			{
				await NotificationDialogService.Show(verificationResult.ErrorMessage,
													 Captions.ND_OkButtonCaption);
			}			
		}

		private async Task DoStartWithProgressFromString (string progressString)
		{
			var loadingResult = await TryToGetUninitializedBot();

			if (loadingResult == null)			
				return;
			
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

			if (verificationResult.Result == VerificationResult.Valid)
			{
				applicationSettingsRepository.LastUsedBotPath = DllPathInput;
				gameService.CreateGame(loadingResult.UninitializedBot,
					                   loadingResult.BotName,
					                   new GameConstraints(TimeSpan.FromSeconds(Constants.GameConstraint.BotThinkingTimeSeconds),
					                   	                   Constants.GameConstraint.MaximalMovesPerGame),
					                   initialProgress);
			}
			else
			{
				await NotificationDialogService.Show(verificationResult.ErrorMessage,
													 Captions.ND_OkButtonCaption);
			}			
		}

		public string BrowseForBotButtonToolTipCaption          => Captions.PvB_BrowseForBotButtonToolTipCaption;
		public string StartGameButtonToolTipCaption             => Captions.PvB_StartGameButtonToolTipCaption;
		public string StartWithProgressGameButtonToolTipCaption => Captions.PvB_StartWithProgressGameButtonToolTipCaption;
		public string OpenInfoButtonToolTipCaption              => Captions.PvB_OpenInfoButtonToolTipCaption;
		public string StartGameFromStringButtonCaption          => Captions.PvB_StartGameFromStringButtonCaption;
		public string StartGameFromFileButtonCaption            => Captions.PvB_StartGameFromFileButtonCaption;		
		public string HeaderCaptionPlayer                       => Captions.PvB_HeaderCaptionPlayer;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(BrowseForBotButtonToolTipCaption),
										 nameof(StartGameButtonToolTipCaption),
										 nameof(StartWithProgressGameButtonToolTipCaption),
										 nameof(StartGameFromStringButtonCaption),
										 nameof(StartGameFromFileButtonCaption),
										 nameof(HeaderCaptionPlayer),										 
										 nameof(OpenInfoButtonToolTipCaption));

			if (!IsBotLoaded)
			{
				TopPlayerName = Captions.PvB_NoBotLoadedCaption;
			}
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
