using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.ViewModels.Board
{
	internal class BoardViewModelSampleData : IBoardViewModel
	{
		public BoardViewModelSampleData()
		{
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
			
			BoardSize = new Size(300, 300);
		}

		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public Size BoardSize { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}