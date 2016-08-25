using QCF.GameEngine.Coordination;
using QCF.GameEngine.GameElements;

namespace QCF.GameEngine.Moves
{
	public class FigureMove : Move
	{
		public FigureMove(BoardState stateBeforeMove, Player playerAtMove, 
						  FieldCoordinate oldPosition, FieldCoordinate newPosition)
			: base (stateBeforeMove, playerAtMove)
		{
			OldPosition = oldPosition;
			NewPosition = newPosition;
		}

		public FieldCoordinate OldPosition { get; }
		public FieldCoordinate NewPosition { get; }
	}
}
