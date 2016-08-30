using QCF.Contest.Contracts.GameElements;

namespace QCF.Contest.Contracts.Moves
{
	public class WallMove : Move
	{
		public WallMove(BoardState stateBeforeMove, Player playerAtMove, Wall placedWall)
			: base(stateBeforeMove, playerAtMove)
		{
			PlacedWall = placedWall;
		}

		public Wall PlacedWall { get; }
	}
}
