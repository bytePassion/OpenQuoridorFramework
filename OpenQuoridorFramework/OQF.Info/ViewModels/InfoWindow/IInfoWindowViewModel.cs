using System.Collections.ObjectModel;
using System.Windows.Input;
using OQF.Info.ViewModels.InfoWindow.Helper;
using WpfLib.ViewModelBase;

namespace OQF.Info.ViewModels.InfoWindow
{
	internal interface IInfoWindowViewModel : IViewModel
	{
		ICommand CloseWindow { get; }

		int SelectedPage { get; }

		ObservableCollection<SelectionButtonData> PageSelectionCommands { get; }
	}
}