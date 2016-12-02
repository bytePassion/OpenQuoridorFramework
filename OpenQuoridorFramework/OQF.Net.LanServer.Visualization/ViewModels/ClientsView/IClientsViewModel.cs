using System.Collections.ObjectModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.ClientsView
{
	public interface IClientsViewModel : IViewModel
	{
		ObservableCollection<string> ConnectedClients { get; }
	}
}