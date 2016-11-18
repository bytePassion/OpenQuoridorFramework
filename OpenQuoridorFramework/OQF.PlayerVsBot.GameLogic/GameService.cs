using System;
using System.Threading;
using OQF.AnalysisAndProgress.Enum;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.PlayerVsBot.Contracts;

namespace OQF.PlayerVsBot.GameLogic
{
	public class GameService : IGameService
	{
		
		private readonly bool disableBotTimeout;
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason, Move> WinnerAvailable;

		private IPvBGame currentIpvBGame;		

		public GameService(bool disableBotTimeout)
		{			
			this.disableBotTimeout = disableBotTimeout;
			currentIpvBGame = null;
			CurrentBoardState = null;			
		}

		public BoardState CurrentBoardState { get; private set; }
		

		public void CreateGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, 
							   QProgress initialProgress)
		{
			if (currentIpvBGame != null)
			{
				StopGame();
			}

			currentIpvBGame = disableBotTimeout 
									? new LocalGamePvB(uninitializedBot, 
													   botName, 
													   new GameConstraints(Timeout.InfiniteTimeSpan, 
													   					   gameConstraints.MaximalMovesPerPlayer), 
													   initialProgress)
									: new LocalGamePvB(uninitializedBot, 
													   botName, 
													   gameConstraints, 
													   initialProgress);

			currentIpvBGame.DebugMessageAvailable   += OnDebugMessageAvailable;
			currentIpvBGame.NextBoardstateAvailable += OnNextBoardstateAvailable;
			currentIpvBGame.WinnerAvailable         += OnWinnerAvailable;			
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{			
			WinnerAvailable?.Invoke(player, winningReason, invalidMove);				
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
