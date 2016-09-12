using System;
using System.Threading;
using ConcurrencyLib;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts;
using OQF.GameEngine.Transitions;

namespace OQF.GameEngine.Game
{
	public class GameLoopThreadPvB : IThread
	{
		private class BotsTimeOut : Move
		{
			public BotsTimeOut() : base(null, null) {}
			public override string ToString() => string.Empty;
		}		

		public event Action<BoardState>             NewBoardStateAvailable;
		public event Action<Player, WinningReason>  WinnerAvailable;

		private readonly Timer botTimer;

		private readonly IQuoridorBot bot;
		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly TimeoutBlockingQueue<Move> botMoves;
		private readonly GameConstraints gameConstraints;

		private volatile bool stopRunning;

				
		public GameLoopThreadPvB (IQuoridorBot uninitializedBot, 
							      TimeoutBlockingQueue<Move> humenMoves, 							      
							      GameConstraints gameConstraints)
		{
			bot = uninitializedBot;
			this.humenMoves = humenMoves;

			botMoves = new TimeoutBlockingQueue<Move>(200);

			bot.NextMoveAvailable += OnNextBotMoveAvailable;
			
			this.gameConstraints = gameConstraints;			

			stopRunning = false;
			IsRunning = false;

			botTimer = new Timer(TimerTick, null, Timeout.Infinite, Timeout.Infinite);			
		}

		private void TimerTick(object state)
		{
			botMoves.Put(new BotsTimeOut());
		}

		private void OnNextBotMoveAvailable(Move move)
		{
			botMoves.Put(move);
			botTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		public void Run ()
		{
			IsRunning = true;

			var computerPlayer = new Player(PlayerType.TopPlayer);
			var humanPlayer    = new Player(PlayerType.BottomPlayer);

			bot.Init(computerPlayer, gameConstraints);

			var currentBoardState = BoardStateTransition.CreateInitialBoadState(computerPlayer, humanPlayer);

			NewBoardStateAvailable?.Invoke(currentBoardState);

			var moveCounter = 0;

			while (!stopRunning)
			{
				if (moveCounter >= gameConstraints.MaximalMovesPerPlayer)
				{
					WinnerAvailable?.Invoke(computerPlayer, WinningReason.MaximumOfMovesExceded);
				}

				var nextHumanMove = PickHumanMove();

				if (nextHumanMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextHumanMove))
				{
					WinnerAvailable?.Invoke(computerPlayer, WinningReason.InvalidMove);
					break;
				}								

				currentBoardState = currentBoardState.ApplyMove(nextHumanMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				if (nextHumanMove is Capitulation)
				{
					WinnerAvailable?.Invoke(computerPlayer, WinningReason.Capitulation);
				}

				var winner = GameAnalysis.CheckWinningCondition(currentBoardState);
				if (winner != null)
				{
					WinnerAvailable?.Invoke(winner, WinningReason.RegularQuoridorWin);
					break;
				}								
				
				var nextBotMove = GetBotMove(currentBoardState);

				if (nextBotMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextBotMove))
				{
					WinnerAvailable?.Invoke(humanPlayer, WinningReason.InvalidMove);
					break;
				}

				currentBoardState = currentBoardState.ApplyMove(nextBotMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				if (nextBotMove is Capitulation)
				{
					WinnerAvailable?.Invoke(humanPlayer, WinningReason.Capitulation);
				}
				
				var winner2 = GameAnalysis.CheckWinningCondition(currentBoardState);
				if (winner2 != null)
				{
					WinnerAvailable?.Invoke(winner2, WinningReason.RegularQuoridorWin);
					break;
				}

				moveCounter++;
			}

			IsRunning = false;
			bot.NextMoveAvailable -= OnNextBotMoveAvailable;
		}

		private Move GetBotMove(BoardState currentBoardState)
		{
			botMoves.Clear();
			botTimer.Change(gameConstraints.MaximalComputingTimePerMove, TimeSpan.Zero);
			bot.DoMove(currentBoardState);			

			Move nextMove;
			while ((nextMove = botMoves.TimeoutTake()) == null)
			{
				if (stopRunning)
					return null;
			}

			if (nextMove is BotsTimeOut)
			{				
				WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player, WinningReason.AiThougtMoreThanAnMinute);
				return null;
			}
				
			return nextMove;
		}

		private Move PickHumanMove()
		{
			Move nextMove;

			while ((nextMove = humenMoves.TimeoutTake()) == null)
			{				
				if (stopRunning)
					return null;
			}

			botTimer.Change(Timeout.Infinite, Timeout.Infinite);

			return nextMove;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}