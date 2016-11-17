using System;
using System.Collections.Generic;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Replay;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.ProgressUtils;

namespace OQF.GameEngine.Replay
{
	public class ReplayService : IReplayService
	{
		public event Action<BoardState> NewBoardStateAvailable;

		private BoardState currentBoardState;

		private IList<BoardState> allReplayStates;
		

		private int currentReplayIndex;

		public ReplayService()
		{
			currentBoardState = null;
		}
		

		public void NewReplay(QProgress progress)
		{
			BuildReplayStates(progress);
			ShowReplayState(0);			
		}

		public void NextMove()                { ShowReplayState(currentReplayIndex+1); }
		public void PreviousMove()            { ShowReplayState(currentReplayIndex-1); }
		public void JumpToMove(int moveIndex) { ShowReplayState(moveIndex);            }

		public BoardState GetCurrentBoardState () => currentBoardState;

		private void ShowReplayState(int index)
		{
			currentReplayIndex = index;

			var nextState = allReplayStates[currentReplayIndex];

			currentBoardState = nextState;
			NewBoardStateAvailable?.Invoke(nextState);
		}

		private void BuildReplayStates(QProgress progress)
		{
			allReplayStates = new List<BoardState>();

			var topPlayer = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);

			var boardState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);
			allReplayStates.Add(boardState);

			var playerAtMove = bottomPlayer;

			foreach (var nextMove in progress.Moves)
			{
				boardState = boardState.ApplyMove(nextMove);
				allReplayStates.Add(boardState);

				playerAtMove = playerAtMove == topPlayer ? bottomPlayer : topPlayer;
			}
		}
	}
}