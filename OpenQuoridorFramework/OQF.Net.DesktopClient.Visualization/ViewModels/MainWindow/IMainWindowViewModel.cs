using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		ICommand ConnectToServer { get; }

		string ServerAddress { get; set; }

		string Response { get; }
	}
}