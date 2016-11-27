using System.Windows;
using Lib.Wpf;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private INetworkGameService networkGameService;

		public void BuildAndStart (StartupEventArgs startupEventArgs)
		{
			networkGameService = new NetworkGameService();

			var mainWindowViewModel = new MainWindowViewModel(networkGameService);

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp (ExitEventArgs exitEventArgs)
		{
			networkGameService.Dissconnect();
		}
	}
}
