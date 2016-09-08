using System;
using System.Windows;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;

namespace OQF.SingleGameVisualization.Services
{
	internal class GameService : IGameService
	{
		private readonly IGameFactory gameFactory;
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason> WinnerAvailable;

		private IGame currentGame;		

		public GameService(IGameFactory gameFactory)
		{
			this.gameFactory = gameFactory;
			currentGame = null;
			CurrentBoardState = null;			
		}

		public BoardState CurrentBoardState { get; private set; }
		

		public void CreateGame(string dllPath, GameConstraints gameConstraints)
		{
			if (currentGame != null)
			{
				StopGame();
			}

			currentGame = gameFactory.CreateNewGame(dllPath, gameConstraints);

			currentGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentGame.WinnerAvailable         += OnWinnerAvailable;			
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
			currentGame?.ReportHumanMove(move);
		}

		public void StopGame()
		{
			if (currentGame != null)
			{
				NewBoardStateAvailable?.Invoke(null);

				currentGame.StopGame();

				currentGame.DebugMessageAvailable   -= OnDebugMessageAvailable;
				currentGame.NextBoardstateAvailable -= OnNextBoardstateAvailable;
				currentGame.WinnerAvailable         -= OnWinnerAvailable;

				currentGame = null;
			}
		}
	}
}
