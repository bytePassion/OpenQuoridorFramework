using System.Collections.Generic;
using System.Linq;
using OQF.AnalysisAndProgress.ProgressUtils.Coding;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.BoardStateUtils;

namespace OQF.AnalysisAndProgress.ProgressUtils
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

		public BoardState GetBoardState(Player bottomPlayer, Player topPlayer)
		{
			var boardState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);

			foreach (var move in Moves)
			{
				boardState = boardState.ApplyMove(move);
			}

			return boardState;			
		}
	}
}
