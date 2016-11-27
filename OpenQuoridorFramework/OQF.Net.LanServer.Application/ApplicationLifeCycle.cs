using System.Windows;
using Lib.Wpf;
using OQF.Net.LanServer.NetworkGameLogic.GameServer;
using OQF.Net.LanServer.Visualization.ViewModels.MainWindow;

namespace OQF.Net.LanServer.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			var clientRepository = new ClientRepository();
			var networkGameServer = new NetworkGameServer(clientRepository);

			var mainWindowViewModel = new MainWindowViewModel(networkGameServer);

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp(ExitEventArgs exitEventArgs)
		{
			// Nothing to do
		}
	}
}
