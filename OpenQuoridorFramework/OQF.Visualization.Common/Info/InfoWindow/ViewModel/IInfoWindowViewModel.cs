using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Info.InfoWindow.ViewModel.Helper;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		ICommand CloseWindow { get; }

		int SelectedPage { get; }

		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}