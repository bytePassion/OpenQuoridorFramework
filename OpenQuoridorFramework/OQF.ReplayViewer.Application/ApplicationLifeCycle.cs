using System.Globalization;
using System.Windows;
using bytePassion.Lib.Utils;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
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
			var boardHorizontalLabelingViewModel = new BoardHorizontalLabelingViewModel(null);
			var boardVerticalLabelingViewModel   = new BoardVerticalLabalingViewModel(null);

			var mainWindowViewModel = new MainWindowViewModel(boardViewModel, 
															  languageSelectionViewModel, 
															  boardHorizontalLabelingViewModel,
															  boardVerticalLabelingViewModel,
															  replayService, 
															  applicationSettingsRepository);

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
