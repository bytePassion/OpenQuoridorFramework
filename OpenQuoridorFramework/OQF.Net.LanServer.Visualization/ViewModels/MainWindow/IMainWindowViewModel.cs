using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview;
using OQF.Net.LanServer.Visualization.ViewModels.LogView;

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		IActionBarViewModel     ActionBarViewModel     { get; }
		IConnectionBarViewModel ConnectionBarViewModel { get; }		
		ILogViewModel           LogViewModel           { get; }	
		IGameOverviewModel      GameOverviewModel      { get; }	

		string ServerLogSectionCaption        { get; }
		string GameOverviewSectionCaption     { get; }
		string ConnectedClientsSectionCaption { get; }
	}
}