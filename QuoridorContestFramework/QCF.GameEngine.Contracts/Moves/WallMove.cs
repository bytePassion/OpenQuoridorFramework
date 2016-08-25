using QCF.GameEngine.Contracts.GameElements;

namespace QCF.GameEngine.Contracts.Moves
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
