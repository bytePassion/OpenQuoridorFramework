using System;
using System.Windows;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;
using QCF.GameEngine.Contracts;

namespace QCF.SingleGameVisualization.Services
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
		

		public void CreateGame(string dllPath)
		{
			if (currentGame != null)
			{
				StopGame();
			}

			currentGame = gameFactory.CreateNewGame(dllPath);

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
