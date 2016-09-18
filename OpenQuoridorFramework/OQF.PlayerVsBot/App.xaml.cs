using System.Windows;
using OQF.GameEngine.Contracts;
using OQF.GameEngine.Game;
using OQF.PlayerVsBot.Services;
using OQF.PlayerVsBot.ViewModels.Board;
using OQF.PlayerVsBot.ViewModels.BoardPlacement;
using OQF.PlayerVsBot.ViewModels.MainWindow;
using OQF.PlayerVsBot.Windows;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.PlayerVsBot
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IGameFactory gameFactory = new GameFactory();
			IGameService gameService = new GameService(gameFactory);
			ILastUsedBotService lastUsedBotService = new LastUsedBotService();

			var boardViewModel = new BoardViewModel(gameService);
			var boardPlacementViewModel = new BoardPlacementViewModel(gameService, gameFactory);
			var languageSelectionViewModel = new LanguageSelectionViewModel();

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, 
															  boardPlacementViewModel, 
															  languageSelectionViewModel,
															  gameService, 
															  lastUsedBotService);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};
			
			mainWindow.ShowDialog();

			gameService.StopGame();
		}
	}
}
