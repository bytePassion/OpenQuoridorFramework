using System;
using System.Collections.Generic;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.ReplayViewer.Contracts;
using OQF.Utils.BoardStateUtils;

namespace OQF.ReplayViewer.GameLogic
{
	public class ReplayService : IReplayService
	{
		public event Action<BoardState> NewBoardStateAvailable;

		public BoardState CurrentBoardState
		{
			get { return currentBoardState; }
			private set
			{
				if (value != currentBoardState)
				{
					currentBoardState = value;
					NewBoardStateAvailable?.Invoke(currentBoardState);
				}
				
			}
		}


		private IList<BoardState> allReplayStates;
		

		private int currentReplayIndex;
		private BoardState currentBoardState;

		public ReplayService()
		{
			CurrentBoardState = null;
			MoveCount = -1;
		}
		

		public void NewReplay(QProgress progress)
		{
			BuildReplayStates(progress);
			MoveCount = allReplayStates.Count;
			ShowReplayState(0);			
		}

		public int MoveCount { get; private set; }

		public void NextMove()                { ShowReplayState(currentReplayIndex+1); }
		public void PreviousMove()            { ShowReplayState(currentReplayIndex-1); }
		public void JumpToMove(int moveIndex) { ShowReplayState(moveIndex);            }
		

		private void ShowReplayState(int index)
		{
			if (index >= 0 && index < allReplayStates.Count)
			{
				currentReplayIndex = index;
				var nextState = allReplayStates[currentReplayIndex];
				CurrentBoardState = nextState;

			}
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