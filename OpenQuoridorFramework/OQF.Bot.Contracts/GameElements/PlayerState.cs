using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.GameElements
{
	/// <summary>
	/// The PlayerState-class represents the state of a player at a specific moment in the game
	/// </summary>

	public class PlayerState
	{
	    public PlayerState(Player player, FieldCoordinate position, int wallsToPlace)
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
