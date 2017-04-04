using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.Commands.Updater;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Utils;
using OQF.Net.LanServer.Contracts;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar
{
	public class ConnectionBarViewModel : ViewModel, IConnectionBarViewModel
	{
		private readonly INetworkGameServer networkGameServer;
		private bool isServerActive;

		public ConnectionBarViewModel(INetworkGameServer networkGameServer)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.networkGameServer = networkGameServer;
			ActivateServer   = new Command(DoActivate,   () => !IsServerActive, new PropertyChangedCommandUpdater(this, nameof(IsServerActive)));
			DeactivateServer = new Command(DoDeactivate, () =>  IsServerActive, new PropertyChangedCommandUpdater(this, nameof(IsServerActive)));

			AvailableIpAddresses = IpAddressCatcher.GetAllAvailableLocalIpAddresses()
												   .Select(address => address.Identifier.ToString())
												   .ToObservableCollection();

			SelectedIpAddress = AvailableIpAddresses.First();
		}		

		public ICommand ActivateServer   { get; }
		public ICommand DeactivateServer { get; }

		public string SelectedIpAddress { get; set; }

		public bool IsServerActive
		{
			get { return isServerActive; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isServerActive, value); }
		}

		public ObservableCollection<string> AvailableIpAddresses { get; }

		public string ActivateButtonCaption    => Captions.LSv_ActivateButtonCaption;
		public string DeactivateButtonCaption  => Captions.LSv_DeactivateButtonCaption;
		public string SelectServerAddressPromt => Captions.LSv_SelectServerAddressPromt;

		private async void DoDeactivate ()
		{
			var userConfirmLeaving = await YesNoDialogService.Show(Captions.LSv_DeactivateServerConfirmationPromt);

			if (userConfirmLeaving)
			{
				networkGameServer.Deactivate();
				IsServerActive = false;
			}				
		}

		private void DoActivate ()
		{
			networkGameServer.Activate(new Address(new TcpIpProtocol(),
												   AddressIdentifier.GetIpAddressIdentifierFromString(SelectedIpAddress)));
			IsServerActive = true;
		}

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(ActivateButtonCaption),
										 nameof(DeactivateButtonCaption),
										 nameof(SelectServerAddressPromt));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
