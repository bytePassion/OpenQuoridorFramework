﻿using System;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;

namespace OQF.SingleGameVisualization.Services
{
	internal interface IGameService
	{
		event Action<BoardState>            NewBoardStateAvailable;
		event Action<string>                NewDebugMsgAvailable;
		event Action<Player, WinningReason> WinnerAvailable;

		BoardState CurrentBoardState { get; }		
		
		void CreateGame(string dllPath, GameConstraints gameConstraints);
		void ReportHumanMove(Move move);
		void StopGame();
	}
}