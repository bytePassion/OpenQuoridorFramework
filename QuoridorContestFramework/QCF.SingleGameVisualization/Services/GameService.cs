﻿using System;
using QCF.GameEngine;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;

namespace QCF.SingleGameVisualization.Services
{
	internal class GameService : IGameService
	{
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player> WinnerAvailable;

		private IGame currentGame;		

		public GameService()
		{
			currentGame = null;			
		}

		public void CreateGame(string dllPath)
		{
			if (currentGame != null)
			{
				StopGame();
			}

			currentGame = new LocalGamePvC(dllPath);

			currentGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentGame.WinnerAvailable         += OnWinnerAvailable;
		}

		private void OnWinnerAvailable(Player player)
		{
			WinnerAvailable?.Invoke(player);
		}

		private void OnNextBoardstateAvailable(BoardState boardState)
		{
			NewBoardStateAvailable?.Invoke(boardState);
		}

		private void OnDebugMessageAvailable(string s)
		{
			NewDebugMsgAvailable?.Invoke(s);
		}

		public void ReportHumanMove(Move move)
		{
			currentGame?.ReportHumanMove(move);
		}

		public void StopGame()
		{
			if (currentGame != null)
			{
				currentGame.StopGame();

				currentGame.DebugMessageAvailable   -= OnDebugMessageAvailable;
				currentGame.NextBoardstateAvailable -= OnNextBoardstateAvailable;
				currentGame.WinnerAvailable         -= OnWinnerAvailable;
			}
		}
	}
}
