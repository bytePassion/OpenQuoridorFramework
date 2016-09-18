using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.ViewModels.InfoWindow.Helper;

namespace OQF.Visualization.Common.ViewModels.InfoWindow
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		ICommand CloseWindow { get; }

		int SelectedPage { get; }

		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}