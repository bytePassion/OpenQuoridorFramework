using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.LanMessaging.AddressTypes;


namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameService networkGameService;
		private string response;

		public MainWindowViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;
			networkGameService.GotConnected += OnGotConnected;
			ConnectToServer = new Command(DoConnect);
		}

		private void OnGotConnected()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = "positive";
			});
		}

		public ICommand ConnectToServer { get; }

		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }

		public string Response
		{
			get { return response; }
			private set { PropertyChanged.ChangeAndNotify(this, ref response, value); }
		}

		private void DoConnect()
		{
			networkGameService.ConnectToServer(AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress));
		}

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
