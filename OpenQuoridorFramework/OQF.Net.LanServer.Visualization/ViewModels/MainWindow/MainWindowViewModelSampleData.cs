using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			Text = "blubb";
		}

		public string Text { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}