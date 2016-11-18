﻿using System;
using OQF.AnalysisAndProgress.Enum;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.PlayerVsBot.Contracts
{
	public interface IGameService
	{
		event Action<BoardState>                  NewBoardStateAvailable;
		event Action<string>                      NewDebugMsgAvailable;
		event Action<Player, WinningReason, Move> WinnerAvailable;

		BoardState CurrentBoardState { get; }		
		
		void CreateGame(IQuoridorBot uninitializedBot, 
						string botName, 
						GameConstraints gameConstraints, 
						QProgress initialProgress = null);

		void ReportHumanMove(Move move);
		void StopGame();
	}
}