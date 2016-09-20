using System.Windows;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel;
using OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.BotVsBotInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.HowToWriteABotPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.PlayerVsBotInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorNotationPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.ReplayViewerInfoPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.TurnamentInfoPage;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;
using OQF.Visualization.Resources;

namespace OQF.Visualization.Common.Info
{
	public static class InfoWindowService
	{
		public static void Show(ApplicationInfo applicationInfo, params InfoPage[] visibleInfoPages)
		{
			var quoridorRulesPageViewModel    = new QuoridorRulesPageViewModel();
			var quoridorNotationPageViewModel = new QuoridorNotationPageViewModel();
			var howToWriteABotPageViewModel   = new HowToWriteABotPageViewModel();
			var botVsBotInfoPageViewModel     = new BotVsBotInfoPageViewModel();
			var playerVsBotInfoPageViewModel  = new PlayerVsBotInfoPageViewModel();
			var replayViewerInfoPageViewModel = new ReplayViewerInfoPageViewModel();
			var turnamentInfoPageViewModel    = new TurnamentInfoPageViewModel();
			var languageSelectionViewModel    = new LanguageSelectionViewModel();
			var aboutPageViewModel            = new AboutPageViewModel(applicationInfo);

			var infoWindowViewModel = new InfoWindowViewModel(visibleInfoPages,
															  languageSelectionViewModel,
															  quoridorRulesPageViewModel,
															  quoridorNotationPageViewModel,
															  howToWriteABotPageViewModel,
															  botVsBotInfoPageViewModel,
															  playerVsBotInfoPageViewModel,
															  replayViewerInfoPageViewModel,
															  turnamentInfoPageViewModel,
															  aboutPageViewModel);

			var infoWindow = new InfoWindow.InfoWindow
			{
				DataContext = infoWindowViewModel,
				Owner  = Application.Current.MainWindow,
				Height = Application.Current.MainWindow.Height,
				Width  = Application.Current.MainWindow.Width
			};

			infoWindow.ShowDialog();

			// cleanup

			quoridorRulesPageViewModel.Dispose();
			quoridorNotationPageViewModel.Dispose();
			howToWriteABotPageViewModel.Dispose();
			botVsBotInfoPageViewModel.Dispose();
			playerVsBotInfoPageViewModel.Dispose();
			replayViewerInfoPageViewModel.Dispose();
			turnamentInfoPageViewModel.Dispose();
			aboutPageViewModel.Dispose();
			languageSelectionViewModel.Dispose();
			infoWindowViewModel.Dispose();
		}
	}
}
