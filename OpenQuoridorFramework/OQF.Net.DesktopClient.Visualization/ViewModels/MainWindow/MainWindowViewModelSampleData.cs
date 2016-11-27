using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			ServerAddress = "10.10.10.10";
			Response = "positive";
			PlayerName = "xelor";
			NewGameName = "myGame01";
		}

		public ICommand ConnectToServer => null;
		public ICommand CreateGame => null;

		public string NewGameName { get; set; }
		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }
		public string Response { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}