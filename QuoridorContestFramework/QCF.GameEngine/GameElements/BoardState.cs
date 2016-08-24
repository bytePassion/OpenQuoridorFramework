using System.Collections.Generic;

namespace QCF.GameEngine.GameElements
{
	public class BoardState
	{
		public BoardState(IReadOnlyList<Wall> placedWalls, PlayerState topPlayer, PlayerState bottomPlayer, Player nextMover)
		{
			PlacedWalls = placedWalls;
			TopPlayer = topPlayer;
			BottomPlayer = bottomPlayer;
			NextMover = nextMover;
		}

		public IReadOnlyList<Wall> PlacedWalls { get; }
		
		public PlayerState TopPlayer    { get; } 
		public PlayerState BottomPlayer { get; }

		public Player NextMover { get; }
	}
}
