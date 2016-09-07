using System.Windows;
using OQF.GameEngine.Contracts;
using OQF.GameEngine.Game;
using OQF.SingleGameVisualization.Services;
using OQF.SingleGameVisualization.ViewModels.Board;
using OQF.SingleGameVisualization.ViewModels.BoardPlacement;
using OQF.SingleGameVisualization.ViewModels.MainWindow;
using OQF.SingleGameVisualization.Windows;

namespace OQF.SingleGameVisualization
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
			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, boardPlacementViewModel, gameService, lastUsedBotService);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};
			
			mainWindow.ShowDialog();

			gameService.StopGame();
		}
	}
}
