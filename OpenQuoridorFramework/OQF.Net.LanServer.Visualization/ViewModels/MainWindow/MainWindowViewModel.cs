using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameServer networkGameServer;

		public MainWindowViewModel(INetworkGameServer networkGameServer, 
								   IActionBarViewModel actionBarViewModel, 
								   IConnectionBarViewModel connectionBarViewModel)
		{
			this.networkGameServer = networkGameServer;
			ActionBarViewModel = actionBarViewModel;
			ConnectionBarViewModel = connectionBarViewModel;

			Output = new ObservableCollection<string>();
			
			networkGameServer.NewOutputAvailable += OnNewOutputAvailable;
		}

		

		private void OnNewOutputAvailable(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Output.Add(s);
			});
			
		}

		public IActionBarViewModel ActionBarViewModel { get; }
		public IConnectionBarViewModel ConnectionBarViewModel { get; }

		
		public ObservableCollection<string> Output { get; }
		

		protected override void CleanUp()
		{
			networkGameServer.NewOutputAvailable -= OnNewOutputAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
