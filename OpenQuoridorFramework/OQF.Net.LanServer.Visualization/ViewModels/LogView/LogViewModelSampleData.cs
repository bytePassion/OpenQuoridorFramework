using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OQF.Net.LanServer.Visualization.ViewModels.LogView
{
	internal class LogViewModelSampleData : ILogViewModel
	{
		public LogViewModelSampleData()
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

		public ObservableCollection<string> Output { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}