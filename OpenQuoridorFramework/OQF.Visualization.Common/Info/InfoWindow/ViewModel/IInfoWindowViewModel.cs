using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;
using OQF.Visualization.Common.Info.Pages.PageViewModels;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		IQuoridorRulesPageViewModel QuoridorRulesPageViewModel { get; }

		ICommand CloseWindow { get; }
		int SelectedPage { get; }
		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}