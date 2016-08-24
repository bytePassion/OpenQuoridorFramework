namespace QCF.GameEngine.Moves
{
	public class WallMove
	{
		public WallMove(WallMove placedWall)
		{
			PlacedWall = placedWall;
		}

		public WallMove PlacedWall { get; }
	}
}
