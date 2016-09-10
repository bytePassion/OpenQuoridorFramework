using System.Windows;
using OQF.GameEngine.Contracts;
using OQF.GameEngine.Game;
using OQF.HumanVsPlayer.Services;
using OQF.HumanVsPlayer.ViewModels.Board;
using OQF.HumanVsPlayer.ViewModels.BoardPlacement;
using OQF.HumanVsPlayer.ViewModels.MainWindow;
using OQF.HumanVsPlayer.Windows;

namespace OQF.HumanVsPlayer
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
