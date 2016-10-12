using System.Collections.ObjectModel;
using System.Windows;
using OQF.CommonUiElements.Info.InfoWindow.ViewModel;
using OQF.CommonUiElements.Info.Pages.PageViewModels;
using OQF.CommonUiElements.Info.Pages.PageViewModels.AboutPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.BotVsBotInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.HowToWriteABotPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.PlayerVsBotInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorNotationPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage;
using OQF.CommonUiElements.Info.Pages.PageViewModels.TournamentInfoPage;
using OQF.Resources;

namespace OQF.CommonUiElements.Info
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
			var turnamentInfoPageViewModel    = new TournamentInfoPageViewModel();			
			var aboutPageViewModel            = new AboutPageViewModel(applicationInfo);

		    var viewModels = new ObservableCollection<IPage>();

		    foreach (var visibleInfoPage in visibleInfoPages)
		    {
		        switch (visibleInfoPage)
		        {
		            case InfoPage.About:                       viewModels.Add(aboutPageViewModel);            break;
                    case InfoPage.BotVsBotApplicationInfo:     viewModels.Add(botVsBotInfoPageViewModel);     break;
                    case InfoPage.HowToWriteABot:              viewModels.Add(howToWriteABotPageViewModel);   break;
                    case InfoPage.PlayerVsBotApplicationInfo:  viewModels.Add(playerVsBotInfoPageViewModel);  break;
                    case InfoPage.QuoridorNotation:            viewModels.Add(quoridorNotationPageViewModel); break;
                    case InfoPage.QuoridorRules:               viewModels.Add(quoridorRulesPageViewModel);    break;
                    case InfoPage.ReplayViewerApplicationInfo: viewModels.Add(replayViewerInfoPageViewModel); break;
                    case InfoPage.TournamentApplicationInfo:   viewModels.Add(turnamentInfoPageViewModel);    break;                  
		        }
		    }

			var infoWindowViewModel = new InfoWindowViewModel(viewModels);

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

			infoWindowViewModel.Dispose();
		}
	}
}
