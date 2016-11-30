using System.Globalization;
using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.ReplayViewer.Contracts;
using OQF.ReplayViewer.GameLogic;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow;
using OQF.ReplayViewer.Visualization.Windows;
using OQF.Utils;

namespace OQF.ReplayViewer.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			IReplayService replayService = new ReplayService();
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardViewModel = new BoardViewModel(replayService);
			var languageSelectionViewModel = new LanguageSelectionViewModel();

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, languageSelectionViewModel, replayService, applicationSettingsRepository);

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
