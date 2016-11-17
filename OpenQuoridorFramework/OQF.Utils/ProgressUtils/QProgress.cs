using System.Collections.Generic;
using OQF.Bot.Contracts.Moves;

namespace OQF.Utils.ProgressUtils
{
	public class QProgress
	{
		internal QProgress(IEnumerable<Move> moves)
		{
			Moves = moves;
		}

		public IEnumerable<Move> Moves { get; }
	}
}
