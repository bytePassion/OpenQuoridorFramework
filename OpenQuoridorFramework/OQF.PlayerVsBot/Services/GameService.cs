using System;
using System.Windows;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Contracts.Games;

namespace OQF.PlayerVsBot.Services
{
	internal class GameService : IGameService
	{
		private readonly IGameFactory gameFactory;
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason, Move> WinnerAvailable;

		private IPvBGame currentIpvBGame;		

		public GameService(IGameFactory gameFactory)
		{
			this.gameFactory = gameFactory;
			currentIpvBGame = null;
			CurrentBoardState = null;			
		}

		public BoardState CurrentBoardState { get; private set; }
		

		public void CreateGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, string initialProgress)
		{
			if (currentIpvBGame != null)
			{
				StopGame();
			}

			currentIpvBGame = gameFactory.CreateNewGame(uninitializedBot, botName, gameConstraints, initialProgress);

			currentIpvBGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentIpvBGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentIpvBGame.WinnerAvailable         += OnWinnerAvailable;			
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				WinnerAvailable?.Invoke(player, winningReason, invalidMove);
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
