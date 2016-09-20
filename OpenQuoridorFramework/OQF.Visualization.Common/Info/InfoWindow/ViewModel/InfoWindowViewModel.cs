using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModel : Lib.Wpf.ViewModelBase.ViewModel, IInfoWindowViewModel
	{
		
		private int selectedPage;

		public InfoWindowViewModel(IEnumerable<InfoPage> visibleInfoPages,
								   ILanguageSelectionViewModel languageSelectionViewModel,
								   IQuoridorRulesPageViewModel quoridorRulesPageViewModel)
		{
			QuoridorRulesPageViewModel = quoridorRulesPageViewModel;
			LanguageSelectionViewModel = languageSelectionViewModel;
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

		public IQuoridorRulesPageViewModel QuoridorRulesPageViewModel { get; }
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand CloseWindow { get; }

		private void DoCloseWindow ()
		{
			Application.Current.Windows
							   .OfType<InfoWindow>()
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
