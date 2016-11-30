using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.CommonUiElements.Board.ViewModels;

namespace OQF.ReplayViewer.Contracts
{
	public interface IReplayService : IBoardStateProvider
	{
		void NewReplay(QProgress progress);

		int MoveCount { get; }

		void NextMove();
		void PreviousMove();
		void JumpToMove(int moveIndex);

	}
}
