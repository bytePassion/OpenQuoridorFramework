using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Tools.SemanticTypes;
using Size = System.Windows.Size;

#pragma warning disable 0067

namespace QCF.SingleGameVisualization.ViewModels.BoardPlacement
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

			BoardSize = new Size(300,300);
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