using System;
using OQF.Bot.Contracts.GameElements;
using OQF.Utils.ProgressUtils;

namespace OQF.GameEngine.Contracts.Replay
{
	public interface IReplayService
	{
		event Action<BoardState> NewBoardStateAvailable;

		BoardState GetCurrentBoardState();

		void NewReplay(QProgress progress);

		void NextMove();
		void PreviousMove();
		void JumpToMove(int moveIndex);

	}
}
