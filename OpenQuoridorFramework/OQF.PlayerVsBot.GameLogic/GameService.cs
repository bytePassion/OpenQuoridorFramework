using System;
using System.Threading;
using Lib.Communication.State;
using Lib.Concurrency;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.PlayerVsBot.Contracts;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.GameLogic
{
	public class GameService : IGameService
	{		
		private readonly bool disableBotTimeout;
		private readonly ISharedStateWriteOnly<bool> isBoardRotatedVariable;

		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<string> NewDebugMsgAvailable;
		public event Action<Player, WinningReason, Move> WinnerAvailable;
		public event Action<GameStatus> NewGameStatusAvailable;

		private TimeoutBlockingQueue<Move> humenMoves;
		private IGameLoopThread gameLoopThread;
		private IQuoridorBot quoridorBot;
		private BoardState currentBoardState;
		private GameStatus currentGameStatus;


		public GameService(bool disableBotTimeout, ISharedStateWriteOnly<bool> isBoardRotatedVariable)
		{			
			this.disableBotTimeout = disableBotTimeout;
			this.isBoardRotatedVariable = isBoardRotatedVariable;
			CurrentBoardState = null;
			gameLoopThread = null;
			CurrentGameStatus = GameStatus.Unloaded;
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


		public GameStatus CurrentGameStatus
		{
			get { return currentGameStatus; }
			private set
			{
				if (currentGameStatus != value)
				{
					currentGameStatus = value;
					NewGameStatusAvailable?.Invoke(currentGameStatus);
				}				
			}
		}

		public PlayerType HumanPlayerPosition { get; private set; }

		public void CreateGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, 
							   PlayerType startingPosition, QProgress initialProgress)
		{
			HumanPlayerPosition = startingPosition;
			isBoardRotatedVariable.Value = startingPosition == PlayerType.TopPlayer;

			if (gameLoopThread != null)
			{
				StopGame();
			}
		
			var finalGameConstraints = disableBotTimeout 
											? new GameConstraints(Timeout.InfiniteTimeSpan,gameConstraints.MaximalMovesPerPlayer)
											: gameConstraints;

			quoridorBot = uninitializedBot;
			quoridorBot.DebugMessageAvailable += OnDebugMessageAvailable;

			humenMoves = new TimeoutBlockingQueue<Move>(200);

			gameLoopThread = startingPosition == PlayerType.BottomPlayer 
										? (IGameLoopThread) new GameLoopThreadPvB(quoridorBot, botName, humenMoves, finalGameConstraints, initialProgress)
										: (IGameLoopThread) new GameLoopThreadBvP(quoridorBot, botName, humenMoves, finalGameConstraints, initialProgress);

			gameLoopThread.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThread.WinnerAvailable        += OnWinnerAvailable;

			CurrentGameStatus = GameStatus.Active;

			new Thread(gameLoopThread.Run).Start();			
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{			
			WinnerAvailable?.Invoke(player, winningReason, invalidMove);
			CurrentGameStatus = GameStatus.Finished;
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
			if (gameLoopThread != null)
			{
				quoridorBot.DebugMessageAvailable -= OnDebugMessageAvailable;

				gameLoopThread.Stop();

				gameLoopThread.NewBoardStateAvailable -= OnNewBoardStateAvailable;
				gameLoopThread.WinnerAvailable -= OnWinnerAvailable;

				gameLoopThread = null;
				CurrentBoardState = null;
			}

			CurrentGameStatus = GameStatus.Unloaded;
		}
	}
}
