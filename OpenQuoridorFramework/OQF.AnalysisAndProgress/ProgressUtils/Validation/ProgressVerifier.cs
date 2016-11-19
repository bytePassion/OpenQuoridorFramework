using System;
using System.Linq;
using OQF.AnalysisAndProgress.Analysis;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils.BoardStateUtils;

namespace OQF.AnalysisAndProgress.ProgressUtils.Validation
{
	public static class ProgressVerifier 
	{
		public static ProgressVerificationResult Verify(QProgress progress, int maxMoves, bool rejectTerminatedGames)
		{			
			var moves = progress.Moves;
			
			if (!moves.Any())
				return new ProgressVerificationResult(VerificationResult.EmptyOrInvalid,
													  $"{Captions.PVR_ErrorMsg_ProgressCannotBeLoaded}" +
													  $"\n\n{Captions.PVR_ErrorMsg_Reason}:" +
													  $"\n{Captions.PVR_EmptyOrInvalid}");


			if ((int)Math.Ceiling(moves.Count() / 2.0) >= maxMoves)
			{
				return new ProgressVerificationResult(VerificationResult.ProgressContainsMoreMovesThanAllowed,
													  $"{Captions.PVR_ErrorMsg_ProgressCannotBeLoaded}" +
													  $"\n\n{Captions.PVR_ErrorMsg_Reason}:" +
													  $"\n{Captions.PVR_ProgressContainsMoreMovesThanAllowed}");				
			}

			var topPlayer    = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);

			var boardState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);

			foreach (var move in moves)
			{
				if (move is Capitulation)
				{
					if (rejectTerminatedGames)
						return new ProgressVerificationResult(VerificationResult.ProgressContainsTerminatedGame,
															  $"{Captions.PVR_ErrorMsg_ProgressCannotBeLoaded}" +
														      $"\n\n{Captions.PVR_ErrorMsg_Reason}:" +
														      $"\n{Captions.PVR_ProgressContainsTerminatedGame}");						
					
					break;
				}

				if (!GameAnalysis.IsMoveLegal(boardState, move))
					return new ProgressVerificationResult(VerificationResult.ProgressContainsInvalidMove,
														  $"{Captions.PVR_ErrorMsg_ProgressCannotBeLoaded}" +
														  $"\n\n{Captions.PVR_ErrorMsg_Reason}:" +
														  $"\n{Captions.PVR_ProgressContainsInvalidMove}");					

				boardState = boardState.ApplyMove(move);

				var winner = GameAnalysis.CheckWinningCondition(boardState);
				if (winner != null)
					if (rejectTerminatedGames)
						return new ProgressVerificationResult(VerificationResult.ProgressContainsTerminatedGame,
															  $"{Captions.PVR_ErrorMsg_ProgressCannotBeLoaded}" +
															  $"\n\n{Captions.PVR_ErrorMsg_Reason}:" +
															  $"\n{Captions.PVR_ProgressContainsTerminatedGame}");
					else
						break;
			}

			return new ProgressVerificationResult(VerificationResult.Valid);
		}
	}
}
