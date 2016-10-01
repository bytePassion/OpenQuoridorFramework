using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.Communication.State;
using Lib.Wpf.Commands;
using OQF.Visualization.Common.Info.Pages.PageViewModels;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;
using OQF.Visualization.Resources.LanguageDictionaries;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModel : Lib.Wpf.ViewModelBase.ViewModel, IInfoWindowViewModel
	{
		
		private int selectedPage;

		private readonly SharedState<InfoPage> selectedPageVariable;

		public InfoWindowViewModel(IEnumerable<IPage> visibleInfoPages,
								   ILanguageSelectionViewModel languageSelectionViewModel)
		{

			LanguageSelectionViewModel = languageSelectionViewModel;

			selectedPageVariable = new SharedState<InfoPage>();

			CloseWindow = new Command(DoCloseWindow);
		    Pages = visibleInfoPages;

		
		}

		public ILanguageSelectionViewModel    LanguageSelectionViewModel    { get; }

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

		protected override void CleanUp()
		{

		}
		public override event PropertyChangedEventHandler PropertyChanged;
		
	}
}
