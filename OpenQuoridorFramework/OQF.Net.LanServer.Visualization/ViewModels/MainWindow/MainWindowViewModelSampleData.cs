using System.ComponentModel;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview;
using OQF.Net.LanServer.Visualization.ViewModels.LogView;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			ActionBarViewModel     = new ActionBarViewModelSampleData();
			ConnectionBarViewModel = new ConnectionBarViewModelSampleData();	
			LogViewModel           = new LogViewModelSampleData();
			GameOverviewModel      = new GameOverviewModelSampleData();

			ServerLogSectionCaption = "Server-Log:";
			GameOverviewSectionCaption = "Game overview:";
			ConnectedClientsSectionCaption = "Connected clients:";
		}

		public IActionBarViewModel     ActionBarViewModel     { get; }
		public IConnectionBarViewModel ConnectionBarViewModel { get; }
		public ILogViewModel           LogViewModel           { get; }
		public IGameOverviewModel      GameOverviewModel      { get; }

		public string ServerLogSectionCaption        { get; }
		public string GameOverviewSectionCaption     { get; }
		public string ConnectedClientsSectionCaption { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}