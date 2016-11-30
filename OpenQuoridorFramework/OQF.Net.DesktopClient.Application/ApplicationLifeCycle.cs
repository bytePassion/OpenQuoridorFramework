using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private INetworkGameService networkGameService;

		public void BuildAndStart (StartupEventArgs startupEventArgs)
		{
			networkGameService = new NetworkGameService();

			var boardPlacementViewModel = new BoardPlacementViewModel(networkGameService);
			var boardViewModel = new BoardViewModel(networkGameService);
			var progressViewModel = new ProgressViewModel(networkGameService);

			var mainWindowViewModel = new MainWindowViewModel(networkGameService, 
															  boardPlacementViewModel, 
															  boardViewModel, 
															  progressViewModel);

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
