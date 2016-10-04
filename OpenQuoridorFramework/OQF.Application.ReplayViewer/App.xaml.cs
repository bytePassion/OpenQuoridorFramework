using System.Windows;
using OQF.GameEngine.Contracts.Replay;
using OQF.GameEngine.Replay;
using OQF.Visualization.ReplayViewer.Services;
using OQF.Visualization.ReplayViewer.ViewModels.Board;
using OQF.Visualization.ReplayViewer.ViewModels.MainWindow;
using OQF.Visualization.ReplayViewer.Windows;

namespace OQF.Application.ReplayViewer
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
