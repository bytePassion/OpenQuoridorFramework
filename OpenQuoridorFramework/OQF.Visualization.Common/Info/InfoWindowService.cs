using System.Windows;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel;
using OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;
using OQF.Visualization.Resources;

namespace OQF.Visualization.Common.Info
{
	public static class InfoWindowService
	{
		public static void Show(ApplicationInfo applicationInfo, params InfoPage[] visibleInfoPages)
		{
			var quoridorRulesPageViewModel = new QuoridorRulesPageViewModel();
			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var aboutPageViewModel = new AboutPageViewModel(applicationInfo);

			var infoWindowViewModel = new InfoWindowViewModel(visibleInfoPages,
															  languageSelectionViewModel,
															  quoridorRulesPageViewModel,
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
			languageSelectionViewModel.Dispose();
			infoWindowViewModel.Dispose();
		}
	}
}
