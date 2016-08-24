using System.Windows;
using QCF.SingleGameVisualization.ViewModels.MainWindow;

namespace QCF.SingleGameVisualization
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
