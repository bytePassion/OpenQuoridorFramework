using System;
using System.Threading;
using Lib.Concurrency;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.PlayerVsBot.Contracts;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.GameLogic
{
	public class GameService : IGameService
	{		
		private readonly bool disableBotTimeout;

		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason, Move> WinnerAvailable;

		private TimeoutBlockingQueue<Move> humenMoves;
		private GameLoopThreadPvB gameLoopThreadPvB;
		private IQuoridorBot quoridorBot;
		private BoardState currentBoardState;


		public GameService(bool disableBotTimeout)
		{			
			this.disableBotTimeout = disableBotTimeout;			
			CurrentBoardState = null;
			gameLoopThreadPvB = null;
		}

		public BoardState CurrentBoardState
		{
			get { return currentBoardState; }
			private set
			{
				if (value != currentBoardState)
				{
					currentBoardState = value;
					NewBoardStateAvailable?.Invoke(currentBoardState);
				}				
			}
		}


		public void CreateGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, 
							   QProgress initialProgress)
		{
			if (gameLoopThreadPvB != null)
			{
				StopGame();
			}
		
			var finalGameConstraints = disableBotTimeout 
											? new GameConstraints(Timeout.InfiniteTimeSpan,gameConstraints.MaximalMovesPerPlayer)
											: gameConstraints;

			quoridorBot = uninitializedBot;
			quoridorBot.DebugMessageAvailable += OnDebugMessageAvailable;

			humenMoves = new TimeoutBlockingQueue<Move>(200);

			gameLoopThreadPvB = new GameLoopThreadPvB(quoridorBot, 
													  botName, 
													  humenMoves, 
													  finalGameConstraints, 
													  initialProgress);

			gameLoopThreadPvB.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThreadPvB.WinnerAvailable += OnWinnerAvailable;

			new Thread(gameLoopThreadPvB.Run).Start();


		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{			
			WinnerAvailable?.Invoke(player, winningReason, invalidMove);				
		}

		private void OnNewBoardStateAvailable (BoardState boardState)
		{
			CurrentBoardState = boardState;			
		}		

		private void OnDebugMessageAvailable(string s)
		{						
			NewDebugMsgAvailable?.Invoke(s);					
		}

		public void ReportHumanMove(Move move)
		{
			humenMoves.Put(move);
		}

		public void StopGame()
		{
			if (gameLoopThreadPvB != null)
			{
				quoridorBot.DebugMessageAvailable -= OnDebugMessageAvailable;

				gameLoopThreadPvB.Stop();

				gameLoopThreadPvB.NewBoardStateAvailable -= OnNewBoardStateAvailable;
				gameLoopThreadPvB.WinnerAvailable -= OnWinnerAvailable;

				gameLoopThreadPvB = null;
				CurrentBoardState = null;
			}
		}
	}
}
