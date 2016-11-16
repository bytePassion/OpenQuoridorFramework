using System.Globalization;
using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Factories;
using OQF.PlayerVsBot.Contracts.Settings;
using OQF.PlayerVsBot.Visualization.Services;
using OQF.PlayerVsBot.Visualization.ViewModels.Board;
using OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement;
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

			IGameFactory gameFactory = new GameFactory();
			gameService = new GameService(gameFactory, commandLineArguments.DisableBotTimeout);
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardViewModel = new BoardViewModel(gameService);
			var boardPlacementViewModel = new BoardPlacementViewModel(gameService, gameFactory);
			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var progressFileVerifierFactory = new ProgressVerifierFactory();

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel,
															  boardPlacementViewModel,
															  languageSelectionViewModel,
															  gameService,
															  applicationSettingsRepository,
															  progressFileVerifierFactory,
															  commandLineArguments.DisableClosingDialogs);

			if (!string.IsNullOrWhiteSpace(commandLineArguments.BotPath))
			{
				var dllPath = commandLineArguments.BotPath;
				mainWindowViewModel.DllPathInput = dllPath;

				if (string.IsNullOrWhiteSpace(commandLineArguments.ProgressFilePath))
				{
					if (mainWindowViewModel.Start.CanExecute(null))
						mainWindowViewModel.Start.Execute(null);
					else
					{
						MessageBox.Show("startup-error");
						return;
					}
				}
				else
				{
					if (mainWindowViewModel.StartWithProgress.CanExecute(commandLineArguments.ProgressFilePath))
						mainWindowViewModel.StartWithProgress.Execute(commandLineArguments.ProgressFilePath);
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
