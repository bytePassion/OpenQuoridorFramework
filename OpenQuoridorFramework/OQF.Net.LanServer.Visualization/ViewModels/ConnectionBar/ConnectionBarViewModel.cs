using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Utils;
using OQF.Net.LanServer.Contracts;

namespace OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar
{
	public class ConnectionBarViewModel : ViewModel, IConnectionBarViewModel
	{
		private readonly INetworkGameServer networkGameServer;

		public ConnectionBarViewModel(INetworkGameServer networkGameServer)
		{
			this.networkGameServer = networkGameServer;
			ActivateServer   = new Command(DoActivate);
			DeactivateServer = new Command(DoDeactivate);

			AvailableIpAddresses = IpAddressCatcher.GetAllAvailableLocalIpAddresses()
												   .Select(address => address.Identifier.ToString())
												   .ToObservableCollection();

			SelectedIpAddress = AvailableIpAddresses.First();
		}

		public ICommand ActivateServer   { get; }
		public ICommand DeactivateServer { get; }

		public string SelectedIpAddress { get; set; }

		public ObservableCollection<string> AvailableIpAddresses { get; }

		private void DoDeactivate ()
		{
			networkGameServer.Deactivate();
		}

		private void DoActivate ()
		{
			networkGameServer.Activate(new Address(new TcpIpProtocol(),
												   AddressIdentifier.GetIpAddressIdentifierFromString(SelectedIpAddress)));
		}

		protected override void CleanUp()
		{			
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
