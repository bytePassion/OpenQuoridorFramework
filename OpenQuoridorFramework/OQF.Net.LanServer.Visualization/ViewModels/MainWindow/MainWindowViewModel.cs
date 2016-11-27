using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Utils;
using OQF.Net.LanServer.Contracts;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameServer networkGameServer;

		public MainWindowViewModel(INetworkGameServer networkGameServer)
		{
			this.networkGameServer = networkGameServer;
			Output = new ObservableCollection<string>();

			ActivateServer   = new Command(DoActivate);
			DeactivateServer = new Command(DoDeactivate);
			
			AvailableIpAddresses = IpAddressCatcher.GetAllAvailableLocalIpAddresses()
					                               .Select(address => address.Identifier.ToString())
					                               .ToObservableCollection();

			SelectedIpAddress = AvailableIpAddresses.First();

			networkGameServer.NewOutputAvailable += OnNewOutputAvailable;
		}

		private void DoDeactivate()
		{
			networkGameServer.Deactivate();
		}

		private void DoActivate()
		{
			networkGameServer.Activate(new Address(new TcpIpProtocol(), 
												  AddressIdentifier.GetIpAddressIdentifierFromString(SelectedIpAddress)));
		}

		private void OnNewOutputAvailable(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Output.Add(s);
			});
			
		}

		public ICommand ActivateServer   { get; }
		public ICommand DeactivateServer { get; }
		public string SelectedIpAddress { get; set; }

		public ObservableCollection<string> Output { get; }
		public ObservableCollection<string> AvailableIpAddresses { get; }

		protected override void CleanUp()
		{
			networkGameServer.NewOutputAvailable -= OnNewOutputAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
