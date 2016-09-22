using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.GameElements
{
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
