using System.Windows;
using QCF.GameEngine.Contracts.GameElements;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.SingleGameVisualization.ViewModels.MainWindow;
using QCF.UiTools.Communication.State;

namespace QCF.SingleGameVisualization
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var displayedBoardStateVariable = new SharedState<BoardState>(null);

			var boardViewModel = new BoardViewModel(displayedBoardStateVariable);
			var mainWindowViewModel = new MainWindowViewModel(boardViewModel);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};
			
			mainWindow.ShowDialog();
		}
	}
}
