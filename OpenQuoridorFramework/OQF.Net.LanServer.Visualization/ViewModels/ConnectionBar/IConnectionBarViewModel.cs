using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar
{
	public interface IConnectionBarViewModel : IViewModel
	{
		ICommand ActivateServer { get; }
		ICommand DeactivateServer { get; }

		string SelectedIpAddress { get; set; }

		bool IsServerActive { get; }

		ObservableCollection<string> AvailableIpAddresses { get; }
	}
}