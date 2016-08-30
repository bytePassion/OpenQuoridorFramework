using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.Tools.FrameworkExtensions;
using QCF.Tools.WpfTools.Commands;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private string dllPathInput;
		private string moveInput;
		private int bottomPlayerWallCountLeft;
		private int topPlayerWallCountLeft;
		private string bottomPlayerName;
		private string topPlayerName;
		

		public MainWindowViewModel (IBoardViewModel boardViewModel)
		{
			BoardViewModel = boardViewModel;
			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();			
			
			BrowseDll = new Command(DoBrowseDll);

			IsGameRunning = true;
		}

		public IBoardViewModel BoardViewModel { get; }

		public ICommand Start     { get; }
		public ICommand Restart   { get; }
		public ICommand Stop      { get; }
		public ICommand AboutHelp { get; }
		public ICommand ApplyMove { get; }
		public ICommand BrowseDll { get; }

		public ObservableCollection<string> DebugMessages { get; }
		public ObservableCollection<string> GameProgress  { get; }		

		public bool IsGameRunning { get; }

		public string TopPlayerName
		{
			get { return topPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerName, value); }
		}

		public string BottomPlayerName
		{
			get { return bottomPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref bottomPlayerName, value); }
		}

		public int TopPlayerWallCountLeft
		{
			get { return topPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerWallCountLeft, value); }
		}

		public int BottomPlayerWallCountLeft
		{
			get { return bottomPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref bottomPlayerWallCountLeft, value); }
		}

		public string MoveInput
		{
			get { return moveInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref moveInput, value); }
		}

		public string DllPathInput
		{
			get { return dllPathInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref dllPathInput, value); }
		}		


		private void DoBrowseDll()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "dll|*.dll"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
				if (result.Value)
					DllPathInput = dialog.FileName;
		}


		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}