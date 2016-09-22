using System.Collections.Generic;
using Lib.FrameworkExtension;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.GameEngine.Transitions
{
	public static class BoardStateTransition
	{
		public static BoardState CreateInitialBoadState(Player topPlayer, Player bottomPlayer)
		{
			return new BoardState(new List<Wall>(), 
								  PlayerTransitions.InitialPlayerStateCreation(topPlayer),
								  PlayerTransitions.InitialPlayerStateCreation(bottomPlayer),
								  bottomPlayer,
								  null,
								  null);
		}

		public static BoardState ApplyMove (this BoardState currentBoardState, Move move)
		{
			var wallMove = move as WallMove;
			if (wallMove != null)
			{				
				var topPlayerState = currentBoardState.TopPlayer.Player == currentBoardState.CurrentMover 
											? currentBoardState.TopPlayer.SpendWall()
											: currentBoardState.TopPlayer;

				var bottomPlayerState = currentBoardState.BottomPlayer.Player == currentBoardState.CurrentMover
											? currentBoardState.BottomPlayer.SpendWall()
											: currentBoardState.BottomPlayer;
				
				return new BoardState(currentBoardState.PlacedWalls.Append(wallMove.PlacedWall),
									  topPlayerState,
									  bottomPlayerState,
									  currentBoardState.GetNextMover(),
									  currentBoardState,
									  move);
			}
			else if (move is FigureMove)
			{
				var fieldMove = (FigureMove) move;

				var topPlayerState = currentBoardState.TopPlayer.Player == currentBoardState.CurrentMover
					? currentBoardState.TopPlayer.MovePlayer(fieldMove.NewPosition)
					: currentBoardState.TopPlayer;

				var bottomPlayerState = currentBoardState.BottomPlayer.Player == currentBoardState.CurrentMover
					? currentBoardState.BottomPlayer.MovePlayer(fieldMove.NewPosition)
					: currentBoardState.BottomPlayer;

				return new BoardState(currentBoardState.PlacedWalls,
					topPlayerState,
					bottomPlayerState,
					currentBoardState.GetNextMover(),
					currentBoardState,
					move);
			}
			else
			{													
				return new BoardState(currentBoardState.PlacedWalls,			//
									  currentBoardState.TopPlayer,				//
									  currentBoardState.BottomPlayer,			//	capitulation
									  currentBoardState.GetNextMover(),			//
									  currentBoardState,						//
									  move);									//
			}

		}

		private static Player GetNextMover(this BoardState currentBoardState)
		{
			return currentBoardState.CurrentMover == currentBoardState.TopPlayer.Player
						? currentBoardState.BottomPlayer.Player
						: currentBoardState.TopPlayer.Player;
			
		}
	}
}