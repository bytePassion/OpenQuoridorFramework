using System.Windows;
using Lib.Wpf;
using OQF.Net.LanServer.Visualization.ViewModels.MainWindow;

namespace OQF.Net.LanServer.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			var mainWindowViewModel = new MainWindowViewModel("blubb2");

			var mainWindow = new MainWindow
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
