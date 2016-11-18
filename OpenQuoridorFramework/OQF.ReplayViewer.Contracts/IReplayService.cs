using System;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.GameElements;

namespace OQF.ReplayViewer.Contracts
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
