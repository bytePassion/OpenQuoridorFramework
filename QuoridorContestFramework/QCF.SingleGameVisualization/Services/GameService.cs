using System;
using System.Windows;
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
			IsGameActive = false;
		}

		public BoardState CurrentBoardState { get; private set; }
		public bool IsGameActive { get; private set; }

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

			IsGameActive = true;
		}

		private void OnWinnerAvailable(Player player)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				WinnerAvailable?.Invoke(player);
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
				currentGame.StopGame();

				currentGame.DebugMessageAvailable   -= OnDebugMessageAvailable;
				currentGame.NextBoardstateAvailable -= OnNextBoardstateAvailable;
				currentGame.WinnerAvailable         -= OnWinnerAvailable;
			}
		}
	}
}
