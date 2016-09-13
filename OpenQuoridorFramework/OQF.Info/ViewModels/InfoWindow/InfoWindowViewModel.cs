using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FrameworkExtensionLib;
using OQF.Info.Enum;
using OQF.Info.ViewModels.InfoWindow.Helper;
using WpfLib.Commands;
using WpfLib.ViewModelBase;

namespace OQF.Info.ViewModels.InfoWindow
{
	internal class InfoWindowViewModel : ViewModel, IInfoWindowViewModel
	{
		
		private int selectedPage;

		public InfoWindowViewModel(IEnumerable<InfoPage> visibleInfoPages)
		{
			CloseWindow = new Command(DoCloseWindow);
			PageSelectionCommands = new ObservableCollection<SelectionButtonData>();

			foreach (var page in visibleInfoPages)
			{
				var command = new Command(() =>
				{
					var pageNr = (int) page;
					SelectedPage = pageNr;
				});
				
				PageSelectionCommands.Add(new SelectionButtonData(command, page.ToString()));
			}	
		}

		public ICommand CloseWindow { get; }

		private static void DoCloseWindow ()
		{
			var windows = Application.Current.Windows
											 .OfType<Info.InfoWindow>()
											 .ToList();

			if (windows.Count == 1)
				windows[0].Close();
			else
				throw new Exception("inner error");
		}

		public int SelectedPage
		{
			get { return selectedPage; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
		}

		public ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
		
	}
}
