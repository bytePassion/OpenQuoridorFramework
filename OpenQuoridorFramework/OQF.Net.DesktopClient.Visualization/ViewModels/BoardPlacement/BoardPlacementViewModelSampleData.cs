using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.SemanicTypes;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement
{
	internal class BoardPlacementViewModelSampleData : IBoardPlacementViewModel
	{
		public BoardPlacementViewModelSampleData()
		{
			var player = new Player(PlayerType.BottomPlayer);

			PossibleMoves = new ObservableCollection<PlayerState>
			{
				new PlayerState(player, new FieldCoordinate(XField.D, YField.One), 10),
				new PlayerState(player, new FieldCoordinate(XField.E, YField.Two), 10),
				new PlayerState(player, new FieldCoordinate(XField.F, YField.One), 10),
			};

			PotentialPlacedWall = new ObservableCollection<Wall>
			{
				new Wall(new FieldCoordinate(XField.C, YField.Five), WallOrientation.Horizontal)
			};

			BoardSize = new Size(new Width(300), new Height(300));
		}

		public ICommand BoardClick => null;


		public ObservableCollection<PlayerState> PossibleMoves       { get; }
		public ObservableCollection<Wall>        PotentialPlacedWall { get; }

		public Point MousePosition { set {} }
		public Size BoardSize { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}