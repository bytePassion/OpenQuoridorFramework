using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.GameElements
{
	/// <summary>
	/// The Wall-class represents a wall-element which can be placed on the board with an WallMove
	/// </summary>

	public class Wall
	{
		public Wall(FieldCoordinate topLeft, WallOrientation orientation)
		{
			Orientation = orientation;
			TopLeft = topLeft;
		}

		public FieldCoordinate TopLeft { get; }		
		public WallOrientation Orientation { get; }
	}
}
