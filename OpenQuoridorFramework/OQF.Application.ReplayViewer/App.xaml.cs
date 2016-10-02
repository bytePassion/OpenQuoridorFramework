using System.Windows;
using OQF.ReplayViewer.Services;
using OQF.ReplayViewer.ViewModels.Board;
using OQF.ReplayViewer.ViewModels.MainWindow;
using OQF.ReplayViewer.Windows;

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
