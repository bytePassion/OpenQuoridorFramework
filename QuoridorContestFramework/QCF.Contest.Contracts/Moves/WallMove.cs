using QCF.Contest.Contracts.Coordination;
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

		public override string ToString()
		{
			return PlacedWall.Orientation == WallOrientation.Horizontal
				? $"{PlacedWall.TopLeft}h"
				: $"{PlacedWall.TopLeft}v";
		}
	}
}
