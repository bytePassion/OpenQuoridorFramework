using QCF.GameEngine.Coordination;

namespace QCF.GameEngine.Moves
{
	public class FigureMove
	{
		public FigureMove(FieldCoordinate oldPosition, FieldCoordinate newPosition)
		{
			OldPosition = oldPosition;
			NewPosition = newPosition;
		}

		public FieldCoordinate OldPosition { get; }
		public FieldCoordinate NewPosition { get; }
	}
}
