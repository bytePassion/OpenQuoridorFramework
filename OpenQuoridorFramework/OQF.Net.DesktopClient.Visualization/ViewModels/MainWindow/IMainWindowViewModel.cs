using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		ICommand ConnectToServer { get; }
		ICommand CreateGame { get; }

		string NewGameName { get; set; }
		string ServerAddress { get; set; }
		string PlayerName { get; set; }

		string Response { get; }
	}
}