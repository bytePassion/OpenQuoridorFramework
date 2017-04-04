using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ClientsView;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview;
using OQF.Net.LanServer.Visualization.ViewModels.LogView;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		public MainWindowViewModel(IActionBarViewModel actionBarViewModel, 
								   IConnectionBarViewModel connectionBarViewModel, 
								   ILogViewModel logViewModel, 
								   IGameOverviewModel gameOverviewModel, 
								   IClientsViewModel clientsViewModel)
		{			
			CultureManager.CultureChanged += RefreshCaptions;

			ActionBarViewModel = actionBarViewModel;
			ConnectionBarViewModel = connectionBarViewModel;
			LogViewModel = logViewModel;
			GameOverviewModel = gameOverviewModel;
			ClientsViewModel = clientsViewModel;
		}		

		public IActionBarViewModel     ActionBarViewModel     { get; }
		public IConnectionBarViewModel ConnectionBarViewModel { get; }
		public ILogViewModel           LogViewModel           { get; }
		public IGameOverviewModel      GameOverviewModel      { get; }
		public IClientsViewModel       ClientsViewModel       { get; }

		public string ServerLogSectionCaption        => Captions.LSv_ServerLogSectionCaption;
		public string GameOverviewSectionCaption     => Captions.LSv_GameOverviewSectionCaption;
		public string ConnectedClientsSectionCaption => Captions.LSv_ConnectedClientsSectionCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(ServerLogSectionCaption),
										 nameof(GameOverviewSectionCaption),
										 nameof(ConnectedClientsSectionCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
