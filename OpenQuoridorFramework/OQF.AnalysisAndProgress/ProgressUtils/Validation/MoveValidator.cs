using OQF.AnalysisAndProgress.ProgressUtils.Parsing;

namespace OQF.AnalysisAndProgress.ProgressUtils.Validation
{
	internal static class MoveValidator
	{
		public static bool IsValidMove(string move)
		{
			return MoveParser.GetMove(move) != null;
		}
	}
}