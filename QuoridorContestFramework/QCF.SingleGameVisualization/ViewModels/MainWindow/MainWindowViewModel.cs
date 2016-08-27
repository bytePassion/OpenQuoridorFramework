using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QCF.GameEngine.Contracts.GameElements;
using QCF.UiTools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		public MainWindowViewModel ()
		{
			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();
			VisiblePlayers = new ObservableCollection<PlayerState>();
			VisibleWalls   = new ObservableCollection<Wall>();

			IsGameRunning = true;
		}

		public ICommand Start     { get; }
		public ICommand Restart   { get; }
		public ICommand Stop      { get; }
		public ICommand AboutHelp { get; }
		public ICommand ApplyMove { get; }
		public ICommand BrowseDll { get; }

		public ObservableCollection<string>      DebugMessages  { get; }
		public ObservableCollection<string>      GameProgress   { get; }
		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public bool IsGameRunning { get; }

		public string TopPlayerName    { get; }
		public string BottomPlayerName { get; }

		public int TopPlayerWallCountLeft { get; }
		public int BottomPlayerWallCountLeft { get; }

		public string MoveInput { get; set; }
		public string DllPathInput { get; set; }


		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}