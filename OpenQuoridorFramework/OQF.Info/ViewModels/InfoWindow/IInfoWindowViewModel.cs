using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Info.ViewModels.InfoWindow.Helper;

namespace OQF.Info.ViewModels.InfoWindow
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		ICommand CloseWindow { get; }

		int SelectedPage { get; }

		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}