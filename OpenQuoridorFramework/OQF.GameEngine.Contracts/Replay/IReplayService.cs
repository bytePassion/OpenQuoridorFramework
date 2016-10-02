using System;
using System.Collections.Generic;
using OQF.Bot.Contracts.GameElements;

namespace OQF.GameEngine.Contracts.Replay
{
	public interface IReplayService
	{
		event Action<BoardState> NewBoardStateAvailable;

		BoardState GetCurrentBoardState();

		int NewReplay(IEnumerable<string> allmoves);

		void NextMove();
		void PreviousMove();
		void JumpToMove(int moveIndex);

	}
}
