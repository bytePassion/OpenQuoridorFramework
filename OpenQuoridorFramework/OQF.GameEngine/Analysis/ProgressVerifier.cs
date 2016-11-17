﻿using System;
using System.Linq;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Enums;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.ProgressUtils.Coding;
using OQF.Utils.ProgressUtils.Parsing;

namespace OQF.GameEngine.Analysis
{
	internal class ProgressVerifier : IProgressVerifier
	{
		public ProgressVerificationResult Verify(string progressText, ProgressTextType textType, int maxMoves)
		{
			// TODO: improvable ... hier wird beim entschlüsseln des compressed-Progress einiges zu viel gemacht

			string finalProgressText;

			try
			{
				finalProgressText = textType == ProgressTextType.Readable
										? progressText
										: ProgressCoding.CompressedStringToProgress(progressText);
			}
			catch
			{
				return ProgressVerificationResult.EmptyOrInvalid;
			}
			
			var movesAsString = ParseProgressText.FromFileText(finalProgressText);

			if (!movesAsString.Any())
				return ProgressVerificationResult.EmptyOrInvalid;

			var moves = movesAsString.Select(MoveParser.GetMove);

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
					return ProgressVerificationResult.ProgressContainsTerminatedGame;
				}

				if (!GameAnalysis.IsMoveLegal(boardState, move))
					return ProgressVerificationResult.ProgressContainsInvalidMove;

				boardState = boardState.ApplyMove(move);

				var winner = GameAnalysis.CheckWinningCondition(boardState);
				if (winner != null)
					return ProgressVerificationResult.ProgressContainsTerminatedGame;
			}

			return ProgressVerificationResult.Valid;
		}
	}
}
