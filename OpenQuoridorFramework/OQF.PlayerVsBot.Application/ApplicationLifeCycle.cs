using System.Globalization;
using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.GameLogic;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar;
using OQF.PlayerVsBot.Visualization.ViewModels.DebugMessageView;
using OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar;
using OQF.PlayerVsBot.Visualization.ViewModels.MainWindow;
using OQF.PlayerVsBot.Visualization.Windows;
using OQF.Utils;

namespace OQF.PlayerVsBot.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private IGameService gameService;

		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			var commandLineArguments = CommandLine.Parse(startupEventArgs.Args);
			
			gameService = new GameService(commandLineArguments.DisableBotTimeout);
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardViewModel             = new BoardViewModel(gameService);
			var boardPlacementViewModel    = new BoardPlacementViewModel(gameService);
			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var actionBarViewModel         = new ActionBarViewModel(applicationSettingsRepository, gameService, 
																	languageSelectionViewModel);
			var botStatusBarViewModel      = new BotStatusBarViewModel(gameService);		
			var humanPlayerBarViewModel    = new HumanPlayerBarViewModel(gameService);
			var progressViewModel          = new ProgressViewModel(gameService);
			var debugMessageViewModel      = new DebugMessageViewModel(gameService);

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel,
															  boardPlacementViewModel,															  
															  actionBarViewModel,
															  botStatusBarViewModel,
															  humanPlayerBarViewModel,
															  progressViewModel,
															  debugMessageViewModel,
															  gameService,
															  applicationSettingsRepository,															  
															  commandLineArguments.DisableClosingDialogs);

			if (!string.IsNullOrWhiteSpace(commandLineArguments.BotPath))
			{
				var dllPath = commandLineArguments.BotPath;
				actionBarViewModel.DllPathInput = dllPath;

				if (!string.IsNullOrWhiteSpace(commandLineArguments.ProgressFilePath))
				{
					if (actionBarViewModel.StartWithProgress.CanExecute(null))
					{
						actionBarViewModel.StartWithProgressFromFile.Execute(commandLineArguments.ProgressFilePath);
					}
					else
					{
						MessageBox.Show("startup-error");
						return;
					}
				}
				else if (!string.IsNullOrWhiteSpace(commandLineArguments.ProgressString))
				{
					if (actionBarViewModel.StartWithProgress.CanExecute(null))
					{
						actionBarViewModel.StartWithProgressFromString.Execute(commandLineArguments.ProgressString);
					}
					else
					{
						MessageBox.Show("startup-error");
						return;
					}
				}
				else
				{
					if (actionBarViewModel.Start.CanExecute(null))
					{
						actionBarViewModel.Start.Execute(null);
					}
					else
					{
						MessageBox.Show("startup-error");
						return;
					}					
				}
			}

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp(ExitEventArgs exitEventArgs)
		{
			gameService.StopGame();
		}
	}
}
