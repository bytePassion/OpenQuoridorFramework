using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace ProgressCodingTest.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			Progress = "1. e2 e8\n2. e3 e7";
			ProgressAsString = "rH6Q";
		}

		public ICommand ConvertProgressToString => null;
		public ICommand ConvertStringToProgress => null;

		public string Progress { get; set; }
		public string ProgressAsString { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}