using System.Windows;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel;

namespace OQF.Visualization.Common.Info
{
	public static class InfoWindowService
	{
		public static void Show(params InfoPage[] visibleInfoPages)
		{
			var infoWindowViewModel = new InfoWindowViewModel(visibleInfoPages);

			var infoWindow = new InfoWindow.InfoWindow
			{
				DataContext = infoWindowViewModel,
				Owner  = Application.Current.MainWindow,
				Height = Application.Current.MainWindow.Height,
				Width  = Application.Current.MainWindow.Width
			};

			infoWindow.ShowDialog();

			infoWindowViewModel.Dispose();
		}
	}
}
