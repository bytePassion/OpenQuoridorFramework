using System;
using System.Threading;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Contracts.Games;

namespace OQF.PlayerVsBot.Visualization.Services
{
	public class GameService : IGameService
	{
		private readonly IGameFactory gameFactory;
		private readonly bool disableBotTimeout;
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason, Move> WinnerAvailable;

		private IPvBGame currentIpvBGame;		

		public GameService(IGameFactory gameFactory, bool disableBotTimeout)
		{
			this.gameFactory = gameFactory;
			this.disableBotTimeout = disableBotTimeout;
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

			currentIpvBGame = disableBotTimeout 
									? gameFactory.CreateNewGame(uninitializedBot, 
																botName, 
																new GameConstraints(Timeout.InfiniteTimeSpan, 
																gameConstraints.MaximalMovesPerPlayer), 
																initialProgress)
									: gameFactory.CreateNewGame(uninitializedBot, 
																botName, 
																gameConstraints, 
																initialProgress);

			currentIpvBGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentIpvBGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentIpvBGame.WinnerAvailable         += OnWinnerAvailable;			
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(() =>
			{
				WinnerAvailable?.Invoke(player, winningReason, invalidMove);
			});			
		}

		private void OnNextBoardstateAvailable(BoardState boardState)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(() =>
			{
				CurrentBoardState = boardState;
				NewBoardStateAvailable?.Invoke(boardState);
			});			
		}

		private void OnDebugMessageAvailable(string s)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(() =>
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
