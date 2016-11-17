using System.Collections.Generic;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.Utils.BoardStateUtils
{
	public static class BoardStateExtensions
	{
		public static IEnumerable<Move> GetMoveList(this BoardState boardState)
		{
			var resultList = new List<Move>();

			var currentBoardState = boardState;

			while (currentBoardState != null)
			{
				if (currentBoardState.LastMove != null)
					resultList.Add(currentBoardState.LastMove);

				currentBoardState = currentBoardState.LastBoardState;
			}

			resultList.Reverse();

			return resultList;			
		}
	}
}
