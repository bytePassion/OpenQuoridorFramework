using QCF.GameEngine.Coordination;

namespace QCF.GameEngine.GameElements
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
