using System.Linq;
using System.Text;

namespace OQF.Utils.ProgressCodingUtils
{
	public static class ProgressCoding
	{

		public static string ConvertToProgress (string progressAsString)
		{ 
			var progressAsCompressedString = progressAsString;

			var progressAsNumber = BaseCoding.Decode(progressAsCompressedString);
			var progressAsMoves = MoveListCoding.ConvertBigIntegerToMoveList(progressAsNumber);
			var progressAsStringMoves = progressAsMoves.Select(move => move.ToString());
			var progressTextList = CreateProgressText.FromMoveList(progressAsStringMoves.ToList());

			var progressText = new StringBuilder();

			foreach (var row in progressTextList)
			{
				progressText.Append(row + "\n");
			}

			return progressText.ToString();
		}

		public static string ConvertToString (string progress)
		{
			var progressText = progress;

			var progressAsStringMoves = ParseProgressText.FromFileText(progressText);
			var progressAsMoves = progressAsStringMoves.Select(MoveParser.GetMove);
			var progressAsNumber = MoveListCoding.ConvertMoveListToBigInteger(progressAsMoves);
			var progressAsCompressedString = BaseCoding.Encode(progressAsNumber);

			return progressAsCompressedString;
		}
	}
}
