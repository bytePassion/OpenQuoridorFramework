using System.Windows;
using OQF.GameEngine.Contracts.Replay;
using OQF.GameEngine.Replay;
using OQF.ReplayViewer.Visualization.Services;
using OQF.ReplayViewer.Visualization.ViewModels.Board;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow;
using OQF.ReplayViewer.Visualization.Windows;

namespace OQF.ReplayViewer.Application
{
	public partial class App 
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			IReplayService replayService = new ReplayService();
			ILastPlayedReplayService lastPlayedReplayService = new LastPlayedReplayService();

			var boardViewModel = new BoardViewModel(replayService);
			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, replayService, lastPlayedReplayService);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.ShowDialog();
		}
	}
}
