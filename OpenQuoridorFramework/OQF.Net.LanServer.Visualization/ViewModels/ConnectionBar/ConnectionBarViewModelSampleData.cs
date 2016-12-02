using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar
{
	internal class ConnectionBarViewModelSampleData : IConnectionBarViewModel
	{
		public ConnectionBarViewModelSampleData()
		{
			AvailableIpAddresses = new ObservableCollection<string>
			{
				"192.168.127.23",
				"10.72.30.5"
			};

			SelectedIpAddress = AvailableIpAddresses.First();
			IsServerActive = false;

			ActivateButtonCaption = "Activate";
			DeactivateButtonCaption = "Deactivate";
			SelectServerAddressPromt = "Select server-address:";
		}

		public ICommand ActivateServer   => null;
		public ICommand DeactivateServer => null;

		public string SelectedIpAddress { get; set; }

		public bool IsServerActive { get; }
		public ObservableCollection<string> AvailableIpAddresses { get; }

		public string ActivateButtonCaption    { get; }
		public string DeactivateButtonCaption  { get; }
		public string SelectServerAddressPromt { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}