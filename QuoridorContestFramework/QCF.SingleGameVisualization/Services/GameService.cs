using System;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Game;

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
			CurrentBoardState = null;
		}

		public BoardState CurrentBoardState { get; private set; }

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
			CurrentBoardState = boardState;
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
