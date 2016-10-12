using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.Wpf.Commands;
using OQF.CommonUiElements.Info.Pages.PageViewModels;
using OQF.Resources.LanguageDictionaries;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModel : Lib.Wpf.ViewModelBase.ViewModel, IInfoWindowViewModel
	{				
		public InfoWindowViewModel(IEnumerable<IPage> visibleInfoPages)
		{					
			CloseWindow = new Command(DoCloseWindow);
		    Pages = visibleInfoPages;		
		}		

		public ICommand CloseWindow { get; }

		public string CloseButtonCaption => Captions.IP_CloseButtonCaption;

		private void DoCloseWindow ()
		{
			Application.Current.Windows
							   .OfType<InfoWindow>()
							   .FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
							   ?.Close();			
		}

		

        public IEnumerable<IPage> Pages { get; }

		protected override void CleanUp() {}
		public override event PropertyChangedEventHandler PropertyChanged;
		
	}
}
