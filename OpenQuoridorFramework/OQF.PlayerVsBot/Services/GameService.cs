﻿using System;
using System.Windows;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;

namespace OQF.PlayerVsBot.Services
{
	internal class GameService : IGameService
	{
		private readonly IGameFactory gameFactory;
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason> WinnerAvailable;

		private IPvBGame currentIpvBGame;		

		public GameService(IGameFactory gameFactory)
		{
			this.gameFactory = gameFactory;
			currentIpvBGame = null;
			CurrentBoardState = null;			
		}

		public BoardState CurrentBoardState { get; private set; }
		

		public void CreateGame(IQuoridorBot uninitializedBot, GameConstraints gameConstraints)
		{
			if (currentIpvBGame != null)
			{
				StopGame();
			}

			currentIpvBGame = gameFactory.CreateNewGame(uninitializedBot, gameConstraints);

			currentIpvBGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentIpvBGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentIpvBGame.WinnerAvailable         += OnWinnerAvailable;			
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				WinnerAvailable?.Invoke(player, winningReason);
			});			
		}

		private void OnNextBoardstateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				CurrentBoardState = boardState;
				NewBoardStateAvailable?.Invoke(boardState);
			});			
		}

		private void OnDebugMessageAvailable(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				NewDebugMsgAvailable?.Invoke(s);
			});			
		}

		public void ReportHumanMove(Move move)
		{
			currentIpvBGame?.ReportHumanMove(move);
		}

		public void StopGame()
		{
			if (currentIpvBGame != null)
			{
				NewBoardStateAvailable?.Invoke(null);

				currentIpvBGame.StopGame();

				currentIpvBGame.DebugMessageAvailable   -= OnDebugMessageAvailable;
				currentIpvBGame.NextBoardstateAvailable -= OnNextBoardstateAvailable;
				currentIpvBGame.WinnerAvailable         -= OnWinnerAvailable;

				currentIpvBGame = null;
			}
		}
	}
}