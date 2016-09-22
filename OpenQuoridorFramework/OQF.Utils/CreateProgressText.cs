using System;
using System.Collections.Generic;
using System.Text;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts;

namespace OQF.Utils
{
	public static class CreateProgressText
    {
	    public static string FromBoardState (BoardState boardState)
	    {
		    var moveList = new List<Move>();

		    var currentBoadState = boardState;

		    while (currentBoadState != null)
		    {
				if (currentBoadState.LastBoardState != null)
					moveList.Add(currentBoadState.LastMove);

			    currentBoadState = currentBoadState.LastBoardState;
		    }

		    moveList.Reverse();

			var sb = new StringBuilder();

		    for (var i = 0; i < moveList.Count; i+=2)
		    {
			    sb.Append($"{i/2 + 1}. ");

			    var move1 = moveList[i].ToString();

			    sb.Append($"{move1} ");

				if (i + 1 < moveList.Count)
					sb.Append($"{moveList[i+1]}");

			    sb.Append(Environment.NewLine);
		    }

			return sb.ToString();
	    }

	    public static string AndAppendWinnerAndReason(this string progress, Player winner, WinningReason winningReason, Move invalidMove)
	    {
		    var sb = new StringBuilder();

		    sb.Append(progress);
			sb.Append(Environment.NewLine);

		    var winnerName = string.IsNullOrWhiteSpace(winner.Name)
								? "Player"
								: winner.Name;

			sb.Append($"{winnerName} wins because");

		    switch (winningReason)
		    {
				case WinningReason.ExceedanceOfThoughtTime:
			    {
				    sb.Append(" the oppenden bot exceeded its thinking time");
				    break;
			    }
				case WinningReason.Capitulation:
			    {
					sb.Append(" the oppenden player capitulated");
					break;
			    }
				case WinningReason.InvalidMove:
			    {
					sb.Append($" the oppenden player tryed to do an illegal move [{invalidMove}]");
					break;
			    }
				case WinningReason.ExceedanceOfMaxMoves:
			    {
					sb.Append(" the oppenden player exceeds the maximal moves per player");
					break;
			    }
				case WinningReason.RegularQuoridorWin:
			    {
					sb.Append(" he/she was simply better ;-)");
					break;
			    }
		    }			

		    return sb.ToString();
	    }
    }
}
