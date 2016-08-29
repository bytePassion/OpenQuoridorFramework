using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using QCF.GameEngine.Contracts.Coordination;
using QCF.GameEngine.Contracts.GameElements;
using QCF.UiTools.FrameworkExtensions;
using QCF.UiTools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.Board
{
	internal class BoardViewModel : ViewModel, IBoardViewModel
	{
		private Size boardSize;

		public BoardViewModel()
		{
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

			BoardSize = new Size(100,100);
		}
		
		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public Size BoardSize
		{
			get { return boardSize; }
			set { PropertyChanged.ChangeAndNotify(this, ref boardSize, value); }
		}

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
