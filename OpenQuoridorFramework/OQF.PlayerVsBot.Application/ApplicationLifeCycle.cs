using System.Globalization;
using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Contracts.Settings;
using OQF.PlayerVsBot.GameLogic;
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
			
			gameService = new GameService(commandLineArguments.DisableBotTimeout);
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardViewModel = new BoardViewModel(gameService);
			var boardPlacementViewModel = new BoardPlacementViewModel(gameService);
			var languageSelectionViewModel = new LanguageSelectionViewModel();			

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel,
															  boardPlacementViewModel,
															  languageSelectionViewModel,
															  gameService,
															  applicationSettingsRepository,															  
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
