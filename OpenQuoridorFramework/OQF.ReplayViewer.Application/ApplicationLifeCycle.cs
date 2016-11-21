using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Board.BoardViewModel;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.ReplayViewer.Contracts;
using OQF.ReplayViewer.GameLogic;
using OQF.ReplayViewer.Visualization.Services;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow;
using OQF.ReplayViewer.Visualization.Windows;

namespace OQF.ReplayViewer.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			IReplayService replayService = new ReplayService();
			ILastPlayedReplayService lastPlayedReplayService = new LastPlayedReplayService();

			var boardViewModel = new BoardViewModel(replayService);
			var languageSelectionViewModel = new LanguageSelectionViewModel();

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, languageSelectionViewModel, replayService, lastPlayedReplayService);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp(ExitEventArgs exitEventArgs)
		{
			// Nothing to do
		}
	}
}
