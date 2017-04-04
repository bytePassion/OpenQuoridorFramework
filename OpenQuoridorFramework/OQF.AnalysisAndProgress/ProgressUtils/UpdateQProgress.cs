using bytePassion.Lib.FrameworkExtensions;
using OQF.Bot.Contracts.Moves;

namespace OQF.AnalysisAndProgress.ProgressUtils
{
	public static class UpdateQProgress
	{
		public static QProgress WithNewMove(QProgress progress, Move newMove)
		{
			return CreateQProgress.FromMoveList(progress.Moves.Append(newMove));
		}
	}
}