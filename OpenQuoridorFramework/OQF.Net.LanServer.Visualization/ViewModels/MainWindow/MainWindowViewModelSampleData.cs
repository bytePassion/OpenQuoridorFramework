using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			Output = new ObservableCollection<string>
			{
				"output1",
				"output2",
				"output3",
				"output4",
				"output5"
			};
		}

		public ICommand ActivateServer   => null;
		public ICommand DeactivateServer => null;

		public ObservableCollection<string> Output { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}