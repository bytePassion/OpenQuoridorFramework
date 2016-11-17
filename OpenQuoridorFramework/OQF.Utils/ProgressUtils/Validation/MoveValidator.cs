using OQF.Utils.ProgressUtils.Parsing;

namespace OQF.Utils.ProgressUtils.Validation
{
	public static class MoveValidator
	{
		public static bool IsValidMove(string move)
		{
			return MoveParser.GetMove(move) != null;
		}
	}
}