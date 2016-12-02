using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		IActionBarViewModel ActionBarViewModel { get; }
		IConnectionBarViewModel ConnectionBarViewModel { get; }

		ICommand ActivateServer   { get; }
		ICommand DeactivateServer { get; }

		string SelectedIpAddress { get; set; }

		ObservableCollection<string> Output { get; }
		ObservableCollection<string> AvailableIpAddresses { get; }
	}
}