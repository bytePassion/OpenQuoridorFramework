namespace OQF.Utils
{
	public static class MoveValidator
	{
		public static bool IsValidMove(string move)
		{
			return MoveParser.GetMove(move) != null;
		}
	}
}