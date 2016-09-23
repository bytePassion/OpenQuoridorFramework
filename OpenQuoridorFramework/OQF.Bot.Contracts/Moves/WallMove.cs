using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.Bot.Contracts.Moves
{
	/// <summary>
	/// With this move a player can place a wall-element on the board
	/// </summary>

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
