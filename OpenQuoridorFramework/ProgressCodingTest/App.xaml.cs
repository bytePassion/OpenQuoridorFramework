using System.Windows;
using ProgressCodingTest.ViewModels.MainWindow;

namespace ProgressCodingTest
{
	public partial class App 
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var mainWindowViewModel = new MainWindowViewModel();

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.ShowDialog();
		}
	}
}
