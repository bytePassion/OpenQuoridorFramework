using System;
using System.Linq;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Transitions;
using OQF.Utils;

namespace OQF.GameEngine.Analysis
{
	internal class ProgressFileVerifier : IProgressFileVerifier
	{
		public FileVerificationResult Verify(string progressText, int maxMoves)
		{
			var movesAsString = ParseProgressText.FromFileText(progressText);

			if (!movesAsString.Any())
				return FileVerificationResult.EmptyOrInvalidFile;

			var moves = movesAsString.Select(MoveParser.GetMove);

			if ((int)Math.Ceiling(moves.Count() / 2.0) >= maxMoves)
			{
				return FileVerificationResult.FileContainsMoreMovesThanAllowed;
			}

			var topPlayer    = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);

			var boardState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);

			foreach (var move in moves)
			{
				if (move is Capitulation)
				{
					return FileVerificationResult.FileContainsTerminatedGame;
				}

				if (!GameAnalysis.IsMoveLegal(boardState, move))
					return FileVerificationResult.FileContainsInvalidMove;

				boardState = boardState.ApplyMove(move);

				var winner = GameAnalysis.CheckWinningCondition(boardState);
				if (winner != null)
					return FileVerificationResult.FileContainsTerminatedGame;
			}

			return FileVerificationResult.ValidFile;
		}
	}
}
