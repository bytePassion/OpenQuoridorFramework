using System;
using System.Linq;
using System.Threading;
using Lib.Concurrency;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Transitions;
using OQF.Utils;

namespace OQF.GameEngine.Game
{
	public class GameLoopThreadPvB : IThread
	{
		private class BotsTimeOut : Move
		{			
			public override string ToString() => string.Empty;
		}		

		public event Action<BoardState>             NewBoardStateAvailable;
		public event Action<Player, WinningReason, Move>  WinnerAvailable;

		private readonly Timer botTimer;

		private readonly IQuoridorBot bot;
		private readonly string botName;
		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly TimeoutBlockingQueue<Move> botMoves;
		private readonly GameConstraints gameConstraints;
		private readonly string initialProgress;

		private volatile bool stopRunning;


		private BoardState currentBoardState;
		private Player humanPlayer;
		private Player computerPlayer;

		public GameLoopThreadPvB (IQuoridorBot uninitializedBot, 
								  string botName,
							      TimeoutBlockingQueue<Move> humenMoves, 							      
							      GameConstraints gameConstraints,
								  string initialProgress)
		{
			bot = uninitializedBot;
			this.botName = botName;
			this.humenMoves = humenMoves;

			botMoves = new TimeoutBlockingQueue<Move>(200);

			bot.NextMoveAvailable += OnNextBotMoveAvailable;
			
			this.gameConstraints = gameConstraints;
			this.initialProgress = initialProgress;

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

			computerPlayer = new Player(PlayerType.TopPlayer, botName);
			humanPlayer    = new Player(PlayerType.BottomPlayer);
			
			bot.Init(computerPlayer.PlayerType, gameConstraints);

			currentBoardState = BoardStateTransition.CreateInitialBoadState(computerPlayer, humanPlayer);
			NewBoardStateAvailable?.Invoke(currentBoardState);

			var moveCounter = 0;

			if (initialProgress != null)
			{
				var moves = ParseProgressText.FromFileText(initialProgress)
											.Select(MoveParser.GetMove);				

				foreach (var move in moves)
				{
					currentBoardState = currentBoardState.ApplyMove(move);
					NewBoardStateAvailable?.Invoke(currentBoardState);
				}

				if (moves.Count()%2 == 1)
				{
					var succeedGame = DoBotMove();

					if (!succeedGame)
					{
						IsRunning = false;
						bot.NextMoveAvailable -= OnNextBotMoveAvailable;
						return;
					}
				}

				moveCounter = (int) Math.Ceiling(moves.Count() / 2.0);
			}						

			while (!stopRunning)
			{
				if (moveCounter >= gameConstraints.MaximalMovesPerPlayer)
				{
					WinnerAvailable?.Invoke(computerPlayer, WinningReason.ExceedanceOfMaxMoves, null);
				}

				bool succeedGame;

				succeedGame = DoHumanMove();
				if (!succeedGame)
					break;


				succeedGame = DoBotMove();
				if (!succeedGame)
					break;

				moveCounter++;
			}

			IsRunning = false;
			bot.NextMoveAvailable -= OnNextBotMoveAvailable;
		}

		private bool DoBotMove()
		{
			var nextBotMove = GetBotMove();

			if (nextBotMove == null)
				return false;

			if (!GameAnalysis.IsMoveLegal(currentBoardState, nextBotMove))
			{
				WinnerAvailable?.Invoke(humanPlayer, WinningReason.InvalidMove, nextBotMove);
				return false; 
			}

			currentBoardState = currentBoardState.ApplyMove(nextBotMove);
			NewBoardStateAvailable?.Invoke(currentBoardState);

			if (nextBotMove is Capitulation)
			{
				WinnerAvailable?.Invoke(humanPlayer, WinningReason.Capitulation, null);
			}

			var winner2 = GameAnalysis.CheckWinningCondition(currentBoardState);
			if (winner2 != null)
			{
				WinnerAvailable?.Invoke(winner2, WinningReason.RegularQuoridorWin, null);
				return false;
			}

			return true;
		}

		private bool DoHumanMove()
		{
			var nextHumanMove = PickHumanMove();

			if (nextHumanMove == null)
				return false;

			if (!GameAnalysis.IsMoveLegal(currentBoardState, nextHumanMove))
			{
				WinnerAvailable?.Invoke(computerPlayer, WinningReason.InvalidMove, nextHumanMove);
				return false;
			}

			currentBoardState = currentBoardState.ApplyMove(nextHumanMove);
			NewBoardStateAvailable?.Invoke(currentBoardState);

			if (nextHumanMove is Capitulation)
			{
				WinnerAvailable?.Invoke(computerPlayer, WinningReason.Capitulation, null);
			}

			var winner = GameAnalysis.CheckWinningCondition(currentBoardState);
			if (winner != null)
			{
				WinnerAvailable?.Invoke(winner, WinningReason.RegularQuoridorWin, null);
				return false;
			}

			return true;
		}

		private Move GetBotMove()
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
				WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player, WinningReason.ExceedanceOfThoughtTime, null);
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