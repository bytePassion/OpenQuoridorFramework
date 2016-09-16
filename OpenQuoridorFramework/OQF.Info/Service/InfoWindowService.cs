﻿using System.Windows;
using OQF.Info.Enum;
using OQF.Info.ViewModels.InfoWindow;

namespace OQF.Info.Service
{
	public static class InfoWindowService
	{
		public static void Show(params InfoPage[] visibleInfoPages)
		{
			var infoWindowViewModel = new InfoWindowViewModel(visibleInfoPages);

			var infoWindow = new InfoWindow
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
