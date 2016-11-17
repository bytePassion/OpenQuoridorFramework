using System.Collections.Generic;
using System.Linq;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.ProgressUtils.Coding;

namespace OQF.Utils.ProgressUtils
{
	public class QProgress
	{
		internal QProgress(IEnumerable<Move> moves)
		{
			Moves = moves;
		}

		public IEnumerable<Move> Moves { get; }
		public int MoveCount => Moves.Count();
		public string Compressed => ProgressCoding.ProgressToCompressedString(Moves);
	}
}
