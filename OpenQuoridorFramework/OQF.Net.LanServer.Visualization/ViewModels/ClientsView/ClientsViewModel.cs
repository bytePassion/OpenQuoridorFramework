using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Contracts;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.ClientsView
{
	public class ClientsViewModel : ViewModel, IClientsViewModel
	{
		private readonly IClientRepository clientRepository;

		public ClientsViewModel(IClientRepository clientRepository)
		{
			this.clientRepository = clientRepository;

			ConnectedClients = new ObservableCollection<string>();

			clientRepository.RepositoryChanged += OnRepositoryChanged;
			OnRepositoryChanged();
		}

		private void OnRepositoryChanged()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				ConnectedClients.Clear();

				clientRepository.GetAllClients()
								.Select(client => client.PlayerName)
								.Do(ConnectedClients.Add);
			});
		}

		public ObservableCollection<string> ConnectedClients { get; }

		protected override void CleanUp()
		{
			clientRepository.RepositoryChanged -= OnRepositoryChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
