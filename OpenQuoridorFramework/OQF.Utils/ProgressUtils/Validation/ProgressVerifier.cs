using System;
using System.Linq;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.Analysis;
using OQF.Utils.BoardStateUtils;

namespace OQF.Utils.ProgressUtils.Validation
{
	public static class ProgressVerifier 
	{
		public static ProgressVerificationResult Verify(QProgress progress, int maxMoves, bool rejectTerminatedGames)
		{			
			var moves = progress.Moves;

			if ((int)Math.Ceiling(moves.Count() / 2.0) >= maxMoves)
			{
				return ProgressVerificationResult.ProgressContainsMoreMovesThanAllowed;
			}

			var topPlayer    = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);

			var boardState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);

			foreach (var move in moves)
			{
				if (move is Capitulation)
				{
					if (rejectTerminatedGames)
						return ProgressVerificationResult.ProgressContainsTerminatedGame;
					else
						break;
				}

				if (!GameAnalysis.IsMoveLegal(boardState, move))
					return ProgressVerificationResult.ProgressContainsInvalidMove;

				boardState = boardState.ApplyMove(move);

				var winner = GameAnalysis.CheckWinningCondition(boardState);
				if (winner != null)
					if (rejectTerminatedGames)
						return ProgressVerificationResult.ProgressContainsTerminatedGame;
					else
						break;
			}

			return ProgressVerificationResult.Valid;
		}
	}
}
