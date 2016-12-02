using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			ActionBarViewModel = new ActionBarViewModelSampleData();

			Output = new ObservableCollection<string>
			{
				"output1",
				"output2",
				"output3",
				"output4",
				"output5"
			};

			AvailableIpAddresses = new ObservableCollection<string>
			{
				"192.168.127.23",
				"10.72.30.5"
			};

			SelectedIpAddress = AvailableIpAddresses.First();
		}

		public IActionBarViewModel ActionBarViewModel { get; }

		public ICommand ActivateServer   => null;
		public ICommand DeactivateServer => null;
		public string SelectedIpAddress { get; set; }

		public ObservableCollection<string> Output { get; }
		public ObservableCollection<string> AvailableIpAddresses { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}