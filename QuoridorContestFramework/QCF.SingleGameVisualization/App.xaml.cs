using System.Windows;
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

			IGameService gameService = new GameService();
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
