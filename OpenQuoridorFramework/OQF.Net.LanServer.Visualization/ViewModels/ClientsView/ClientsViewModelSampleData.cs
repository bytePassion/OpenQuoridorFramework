using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.ClientsView
{
	internal class ClientsViewModelSampleData : IClientsViewModel
	{

		public ClientsViewModelSampleData()
		{
			ConnectedClients = new ObservableCollection<string>
			{
				"player1",
				"player2",
				"player3",
				"player4"
			};
		}

		public ObservableCollection<string> ConnectedClients { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}