using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QCF.GameEngine.Contracts.Coordination;
using QCF.GameEngine.Contracts.GameElements;

#pragma warning disable 0067

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			DebugMessages = new ObservableCollection<string>
			{
				"blubb1",
				"blubb2",
				"blubb3",
				"blubb4",
				"blubb5"
			};

			GameProgress = new ObservableCollection<string>
			{
				"1. e2 e8",
				"2. e3 e7"
			};

			VisiblePlayers = new ObservableCollection<PlayerState>
			{
				new PlayerState(new Player(PlayerType.TopPlayer),    new FieldCoordinate(XField.B, YField.Seven), 10),
				new PlayerState(new Player(PlayerType.BottomPlayer), new FieldCoordinate(XField.G, YField.Five),  10)
			};

			VisibleWalls = new ObservableCollection<Wall>
			{
				new Wall(new FieldCoordinate(XField.E, YField.Eight), WallOrientation.Horizontal),
				new Wall(new FieldCoordinate(XField.A, YField.Two),   WallOrientation.Vertical),
				new Wall(new FieldCoordinate(XField.B, YField.Three), WallOrientation.Horizontal),
				new Wall(new FieldCoordinate(XField.F, YField.Four),  WallOrientation.Vertical)
			};

			TopPlayerName    = "PlayerOben";
			BottomPlayerName = "PlayerUnten";

			TopPlayerWallCountLeft    = 10;
			BottomPlayerWallCountLeft = 9;

			MoveInput = "e8";
			DllPathInput = "blubb.dll";

			IsGameRunning = true;
		}

		public ICommand Start      => null;
		public ICommand Restart    => null;
		public ICommand Stop       => null;
		public ICommand AboutHelp  => null;
		public ICommand ApplyMove  => null;
		public ICommand BrowseDll  => null;

		public ObservableCollection<string>      DebugMessages  { get; }
		public ObservableCollection<string>      GameProgress   { get; }
		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public bool IsGameRunning { get; }

		public string TopPlayerName    { get; }
		public string BottomPlayerName { get; }

		public int TopPlayerWallCountLeft   { get; }
		public int BottomPlayerWallCountLeft { get; }

		public string MoveInput { get; set; }
		public string DllPathInput { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}