using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;

namespace OQF.Contest.Contracts.Moves
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
