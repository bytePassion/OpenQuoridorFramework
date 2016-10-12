using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lib.SemanicTypes;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement
{
	internal class BoardPlacementViewModelSampleData : IBoardPlacementViewModel
	{
		public BoardPlacementViewModelSampleData()
		{
			PossibleMoves = new ObservableCollection<PlayerState>
			{
				new PlayerState(null, new FieldCoordinate(XField.D, YField.One), -1),
				new PlayerState(null, new FieldCoordinate(XField.E, YField.Two), -1),
				new PlayerState(null, new FieldCoordinate(XField.F, YField.One), -1),
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