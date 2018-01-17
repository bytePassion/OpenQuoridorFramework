using System.Windows;

namespace RtestApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow();

            mainWindow.Show();
        }
    }
}
