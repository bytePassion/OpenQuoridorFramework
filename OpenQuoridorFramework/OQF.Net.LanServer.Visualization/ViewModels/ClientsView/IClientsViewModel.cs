using System.Collections.ObjectModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace OQF.Net.LanServer.Visualization.ViewModels.ClientsView
{
	public interface IClientsViewModel : IViewModel
	{
		ObservableCollection<string> ConnectedClients { get; }
	}
}