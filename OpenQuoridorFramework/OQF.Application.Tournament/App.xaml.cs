using System.Windows;
using OQF.Application.Tournament.Services;
using OQF.Application.Tournament.ViewModels;

namespace OQF.Application.Tournament
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            var service = new TournamentService();

            var viewModel = new MainViewModel(service);


            mainWindow.DataContext = viewModel;

            mainWindow.ShowDialog();
        }
    }
}
