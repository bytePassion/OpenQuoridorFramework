using System.Collections.Generic;
using QCF.GameEngine.Moves;

namespace QCF.GameEngine.GameElements
{
	public class BoardState
	{
		internal BoardState(IEnumerable<Wall> placedWalls, 
						  PlayerState topPlayer, PlayerState bottomPlayer, 
						  Player currentMover, Move lastMove)
		{
			PlacedWalls = placedWalls;
			TopPlayer = topPlayer;
			BottomPlayer = bottomPlayer;
			CurrentMover = currentMover;
			LastMove = lastMove;
		}

		public IEnumerable<Wall> PlacedWalls { get; }
		
		public PlayerState TopPlayer    { get; } 
		public PlayerState BottomPlayer { get; }

		public Player CurrentMover { get; }
		public Move LastMove { get; }		
	}
}
