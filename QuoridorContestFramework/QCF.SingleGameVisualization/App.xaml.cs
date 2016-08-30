using System.Windows;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Game;
using QCF.SingleGameVisualization.Services;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.SingleGameVisualization.ViewModels.MainWindow;

namespace QCF.SingleGameVisualization
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
			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, gameService, lastUsedBotService);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};
			
			mainWindow.ShowDialog();

			gameService.StopGame();
		}
	}
}
