using System.Globalization;
using System.Windows;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Factories;
using OQF.PlayerVsBot.Services;
using OQF.PlayerVsBot.Services.SettingsRepository;
using OQF.PlayerVsBot.ViewModels.Board;
using OQF.PlayerVsBot.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.ViewModels.MainWindow;
using OQF.PlayerVsBot.Windows;
using OQF.Visualization.Common.Language;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.Application.PlayerVsBot
{
	public partial class App
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			var commandLineArguments = CommandLine.Parse(e.Args);

			IGameFactory gameFactory = new GameFactory();
			IGameService gameService = new GameService(gameFactory, commandLineArguments.DisableBotTimeout);
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardViewModel = new BoardViewModel(gameService);
			var boardPlacementViewModel = new BoardPlacementViewModel(gameService, gameFactory);
			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var progressFileVerifierFactory = new ProgressFileVerifierFactory();

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

				if (mainWindowViewModel.Start.CanExecute(null))
					mainWindowViewModel.Start.Execute(null);
			}

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.ShowDialog();

			gameService.StopGame();
		}
	}
}
