using System.Collections.Generic;
using System.Linq;
using System.Text;
using OQF.AnalysisAndProgress.ProgressUtils.Parsing;
using OQF.Bot.Contracts.Moves;
using OQF.Utils;

namespace OQF.AnalysisAndProgress.ProgressUtils.Coding
{
	internal static class ProgressCoding
	{

		public static string CompressedStringToProgress (string progressAsString)
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

		public static IEnumerable<Move> CompressedStringToMoveList(string progressAsString)
		{
			try
			{
				var progressAsCompressedString = progressAsString;

				var progressAsNumber = BaseCoding.Decode(progressAsCompressedString);
				return MoveListCoding.ConvertBigIntegerToMoveList(progressAsNumber);
			}
			catch
			{
				return new List<Move>();
			}			
		}
		

		public static string ProgressToCompressedString (string progress)
		{
			var progressText = progress;

			var progressAsStringMoves = ParseProgressText.FromFileText(progressText);
			var progressAsMoves = progressAsStringMoves.Select(MoveParser.GetMove);
			var progressAsNumber = MoveListCoding.ConvertMoveListToBigInteger(progressAsMoves);
			var progressAsCompressedString = BaseCoding.Encode(progressAsNumber);

			return progressAsCompressedString;
		}

		public static string ProgressToCompressedString(IEnumerable<Move> progress)
		{
			var progressAsNumber = MoveListCoding.ConvertMoveListToBigInteger(progress);
			var progressAsCompressedString = BaseCoding.Encode(progressAsNumber);

			return progressAsCompressedString;
		}
	}
}
