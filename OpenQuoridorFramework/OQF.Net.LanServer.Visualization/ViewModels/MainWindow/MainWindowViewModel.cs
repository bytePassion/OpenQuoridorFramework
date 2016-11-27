using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
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

			networkGameServer.NewOutputAvailable += OnNewOutputAvailable;
		}

		private void DoDeactivate()
		{
			networkGameServer.Deactivate();
		}

		private void DoActivate()
		{
			networkGameServer.Activate(null);
		}

		private void OnNewOutputAvailable(string s)
		{
			Output.Add(s);
		}

		public ICommand ActivateServer   { get; }
		public ICommand DeactivateServer { get; }

		public ObservableCollection<string> Output { get; }

		protected override void CleanUp()
		{
			networkGameServer.NewOutputAvailable -= OnNewOutputAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
