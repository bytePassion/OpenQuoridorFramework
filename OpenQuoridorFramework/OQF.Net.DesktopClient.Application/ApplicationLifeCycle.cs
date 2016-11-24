using System.Windows;
using Lib.Wpf;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		public void BuildAndStart (StartupEventArgs startupEventArgs)
		{
			var mainWindowViewModel = new MainWindowViewModel("blubb3");

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp (ExitEventArgs exitEventArgs)
		{
			// Nothing to do
		}
	}
}
