using System.Collections.Generic;
using OQF.Contest.Contracts.Moves;

namespace OQF.Contest.Contracts.GameElements
{
	public class BoardState
	{
	    public BoardState(IEnumerable<Wall> placedWalls, 
						  PlayerState topPlayer, PlayerState bottomPlayer, 
						  Player currentMover, 
						  BoardState lastBoardState, Move lastMove)
		{
			PlacedWalls = placedWalls;
			TopPlayer = topPlayer;
			BottomPlayer = bottomPlayer;
			CurrentMover = currentMover;
			LastBoardState = lastBoardState;
			LastMove = lastMove;
		}

		public IEnumerable<Wall> PlacedWalls { get; }
		
		public PlayerState TopPlayer    { get; } 
		public PlayerState BottomPlayer { get; }

		public Player CurrentMover { get; }
		
		public BoardState LastBoardState { get; }
		public Move       LastMove       { get; }
	}
}
