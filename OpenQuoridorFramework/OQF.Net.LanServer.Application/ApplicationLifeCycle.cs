using System.Globalization;
using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.GameServer;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ClientsView;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview;
using OQF.Net.LanServer.Visualization.ViewModels.LogView;
using OQF.Net.LanServer.Visualization.ViewModels.MainWindow;
using OQF.Utils;

namespace OQF.Net.LanServer.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private INetworkGameServer networkGameServer;

		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var clientRepository = new ClientRepository();
			var gameRepository = new GameRepository();

			networkGameServer = new NetworkGameServer(clientRepository, gameRepository);

			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var actionBarViewModel         = new ActionBarViewModel(languageSelectionViewModel);
			var connectionBarViewModel     = new ConnectionBarViewModel(networkGameServer);
			var logViewModel               = new LogViewModel(networkGameServer);
			var gameOverviewModel          = new GameOverviewModel(gameRepository);
			var clientsViewModel           = new ClientsViewModel(clientRepository);

			var mainWindowViewModel = new MainWindowViewModel(actionBarViewModel,
															  connectionBarViewModel,
															  logViewModel,
															  gameOverviewModel,
															  clientsViewModel);

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp(ExitEventArgs exitEventArgs)
		{
			networkGameServer.Deactivate();
		}
	}
}
