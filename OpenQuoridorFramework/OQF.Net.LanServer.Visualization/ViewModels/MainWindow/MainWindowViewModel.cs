using System.ComponentModel;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.ActionBar;
using OQF.Net.LanServer.Visualization.ViewModels.ConnectionBar;
using OQF.Net.LanServer.Visualization.ViewModels.LogView;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		public MainWindowViewModel(IActionBarViewModel actionBarViewModel, 
								   IConnectionBarViewModel connectionBarViewModel, 
								   ILogViewModel logViewModel)
		{			
			ActionBarViewModel = actionBarViewModel;
			ConnectionBarViewModel = connectionBarViewModel;
			LogViewModel = logViewModel;
		}
		
		public IActionBarViewModel ActionBarViewModel { get; }
		public IConnectionBarViewModel ConnectionBarViewModel { get; }
		public ILogViewModel LogViewModel { get; }

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
