using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.Bot.Contracts.Moves
{
	public class WallMove : Move
	{
		public WallMove(Wall placedWall)			
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
