using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;

namespace QCF.Contest.Contracts.Moves
{
	public class FigureMove : Move
	{
		public FigureMove(BoardState stateBeforeMove, Player playerAtMove, 
						  FieldCoordinate newPosition)
			: base (stateBeforeMove, playerAtMove)
		{			
			NewPosition = newPosition;
		}
		
		public FieldCoordinate NewPosition { get; }

		public override string ToString()
		{
			return NewPosition.ToString();
		}
	}
}
