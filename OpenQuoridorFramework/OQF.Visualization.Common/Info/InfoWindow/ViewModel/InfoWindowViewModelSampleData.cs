using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.Wpf.Commands;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;
using OQF.Visualization.Common.Info.Pages.PageViewModels.AboutPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorNotationPage;
using OQF.Visualization.Common.Info.Pages.PageViewModels.QuoridorRulesPage;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

#pragma warning disable 0067

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal class InfoWindowViewModelSampleData : IInfoWindowViewModel
	{
		public InfoWindowViewModelSampleData()
		{
			QuoridorRulesPageViewModel    = new QuoridorRulesPageViewModelSampleData();
			QuoridorNotationPageViewModel = new QuoridorNotationPageViewModelSampleData();
			AboutPageViewModel            = new AboutPageViewModelSampleData();
			LanguageSelectionViewModel    = new LanguageSelectionViewModelSampleData();

			SelectedPage = 2;

			PageSelectionCommands = new ObservableCollection<SelectionButtonData>
			{
				new SelectionButtonData(new Command(() => {}), "blubb1"),
				new SelectionButtonData(new Command(() => {}), "blubb2"),
				new SelectionButtonData(new Command(() => {}), "blubb3")
			};
		}

		public IQuoridorRulesPageViewModel QuoridorRulesPageViewModel { get; }
		public IQuoridorNotationPageViewModel QuoridorNotationPageViewModel { get; }
		public IAboutPageViewModel AboutPageViewModel { get; }
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand CloseWindow => null;
		public int SelectedPage { get; }
		public ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}