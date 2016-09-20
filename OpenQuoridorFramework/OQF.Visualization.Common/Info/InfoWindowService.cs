using System.Windows;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel;
using OQF.Visualization.Common.Info.Pages.PageViewModels;

namespace OQF.Visualization.Common.Info
{
	public static class InfoWindowService
	{
		public static void Show(params InfoPage[] visibleInfoPages)
		{
			var quoridorRulesPageViewModel = new QuoridorRulesPageViewModel();

			var infoWindowViewModel = new InfoWindowViewModel(visibleInfoPages,
															  quoridorRulesPageViewModel);

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
			infoWindowViewModel.Dispose();
		}
	}
}
