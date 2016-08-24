using QCF.GameEngine.Coordination;

namespace QCF.GameEngine.GameElements
{
	public class PlayerState
	{
		internal PlayerState(Player player, FieldCoordinate position, int wallsToPlace)
		{
			Player = player;
			Position = position;
			WallsToPlace = wallsToPlace;
		}

		public Player          Player       { get; }
		public FieldCoordinate Position     { get; }
		public int             WallsToPlace { get; }
	}
}
