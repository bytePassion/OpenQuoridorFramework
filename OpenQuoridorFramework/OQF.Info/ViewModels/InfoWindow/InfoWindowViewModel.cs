﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Info.Enum;
using OQF.Info.ViewModels.InfoWindow.Helper;

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

			PageSelectionCommands.FirstOrDefault()?.Command.Execute(null);
		}

		public ICommand CloseWindow { get; }

		private void DoCloseWindow ()
		{
			Application.Current.Windows
							   .OfType<Info.InfoWindow>()
							   .FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
							   ?.Close();			
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
