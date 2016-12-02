using System.Collections.ObjectModel;
using System.ComponentModel;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			ActionBarViewModel = new ActionBarViewModelSampleData();
			ConnectionBarViewModel = new ConnectionBarViewModelSampleData();

			Output = new ObservableCollection<string>
			{
				"output1",
				"output2",
				"output3",
				"output4",
				"output5"
			};			
		}

		public IActionBarViewModel ActionBarViewModel { get; }
		public IConnectionBarViewModel ConnectionBarViewModel { get; }
		
		public ObservableCollection<string> Output { get; }
		
		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}