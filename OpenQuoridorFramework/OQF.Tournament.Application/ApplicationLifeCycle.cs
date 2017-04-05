using System.Windows;
using Lib.Wpf;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.Tournament.Contracts;
using OQF.Tournament.Services;
using OQF.Tournament.Services.Game;
using OQF.Tournament.Services.Port;
using OQF.Tournament.Services.Processes;
using OQF.Tournament.Visualization.ViewModels.Main;
using OQF.Tournament.Visualization.Views;

namespace OQF.Tournament.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private IProcessService processService;

		public void BuildAndStart(StartupEventArgs startupEventArgs)
		{            			
			var dataLogger = new DataLogger();

			var portService = new PortService();
			var tournamentProcess = new DeathMatch();
			processService = new ProcessService(portService);
			var gameService = new GameService(processService, dataLogger);
			var tournament = new Services.Tournament(tournamentProcess, gameService, dataLogger);

			var boardViewModel = new BoardViewModel(gameService);
			var viewModel = new MainViewModel(tournament, boardViewModel, dataLogger);

			var mainWindow = new MainWindow
			{
				DataContext = viewModel
			};

			mainWindow.Show();
		}

		public void CleanUp(ExitEventArgs exitEventArgs)
		{
			processService?.Dispose();
		}
	}
}
