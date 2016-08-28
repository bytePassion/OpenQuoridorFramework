using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using QCF.GameEngine.Contracts.Coordination;
using QCF.GameEngine.Contracts.GameElements;
using QCF.UiTools.FrameworkExtensions;
using QCF.UiTools.WpfTools.Commands;
using QCF.UiTools.WpfTools.ViewModelBase;

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
		private Size boardSize;

		public MainWindowViewModel ()
		{
			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();
			VisiblePlayers = new ObservableCollection<PlayerState>();
			VisibleWalls   = new ObservableCollection<Wall>();



			VisiblePlayers = new ObservableCollection<PlayerState>
			{
				new PlayerState(new Player(PlayerType.TopPlayer),    new FieldCoordinate(XField.B, YField.Seven), 10),
				new PlayerState(new Player(PlayerType.BottomPlayer), new FieldCoordinate(XField.G, YField.Five),  10),
				new PlayerState(new Player(PlayerType.BottomPlayer), new FieldCoordinate(XField.I, YField.Nine),  10),
				new PlayerState(new Player(PlayerType.BottomPlayer), new FieldCoordinate(XField.I, YField.One),   10),
			};

			

			VisibleWalls = new ObservableCollection<Wall>
			{
				new Wall(new FieldCoordinate(XField.E, YField.Eight), WallOrientation.Horizontal),
				new Wall(new FieldCoordinate(XField.A, YField.Two),   WallOrientation.Horizontal),
				new Wall(new FieldCoordinate(XField.B, YField.Three), WallOrientation.Vertical),
				new Wall(new FieldCoordinate(XField.F, YField.Four),  WallOrientation.Vertical)
			};




			BrowseDll = new Command(DoBrowseDll);

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

		public Size BoardSize
		{
			get { return boardSize; }
			set { PropertyChanged.ChangeAndNotify(this, ref boardSize, value); }
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