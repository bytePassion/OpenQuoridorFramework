using System;
using System.Threading;
using Lib.Concurrency;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Transitions;

namespace OQF.GameEngine.Game
{
	public class GameLoopThreadBvB : IThread
	{
		private class BotsTimeOut : Move
		{			
			public override string ToString() => string.Empty;
		}		

		public event Action<BoardState>             NewBoardStateAvailable;
		public event Action<Player, WinningReason>  WinnerAvailable;

		private readonly Timer bottomPlayerBotTimer;
		private readonly Timer topPlayerBotTimer;

		private readonly IQuoridorBot bottomPlayerBot;
		private readonly IQuoridorBot topPlayerBot;

		private readonly TimeoutBlockingQueue<Move> topPlayerBotMoves;
		private readonly TimeoutBlockingQueue<Move> bottomPlayerBotMoves;
		
		private readonly GameConstraints gameConstraints;

		private volatile bool stopRunning;

		private BoardState currentBoardState;
		

		public GameLoopThreadBvB (IQuoridorBot bottomPlayerBot,
								  IQuoridorBot topPlayerBot, 							      
							      BoardState initialBoardState,
							      GameConstraints gameConstraints)
		{
			
			this.bottomPlayerBot = bottomPlayerBot;
			this.topPlayerBot = topPlayerBot;
			
			topPlayerBotMoves    = new TimeoutBlockingQueue<Move>(200);
			bottomPlayerBotMoves = new TimeoutBlockingQueue<Move>(200);

			bottomPlayerBot.NextMoveAvailable += OnNextBottomPlayerBotMoveAvailable;
			topPlayerBot.NextMoveAvailable    += OnNextTopPlayerBotMoveAvailable;

			currentBoardState = initialBoardState;
			this.gameConstraints = gameConstraints;			

			stopRunning = false;
			IsRunning = false;

			bottomPlayerBotTimer = new Timer(TimerTickForBottomPlayerBot, null, Timeout.Infinite, Timeout.Infinite);
			topPlayerBotTimer    = new Timer(TimerTickForTopPlayerBot,    null, Timeout.Infinite, Timeout.Infinite);
		}

		private void TimerTickForTopPlayerBot(object state)
		{
			topPlayerBotMoves.Put(new BotsTimeOut());
		}

		private void TimerTickForBottomPlayerBot(object state)
		{
			bottomPlayerBotMoves.Put(new BotsTimeOut());
		}

		private void OnNextTopPlayerBotMoveAvailable(Move move)
		{
			topPlayerBotMoves.Put(move);
			topPlayerBotTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void OnNextBottomPlayerBotMoveAvailable(Move move)
		{
			bottomPlayerBotMoves.Put(move);
			bottomPlayerBotTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		

		public void Run ()
		{
			IsRunning = true;			

			NewBoardStateAvailable?.Invoke(currentBoardState);

			var moveCounter = 0;

			while (!stopRunning)
			{
				if (moveCounter >= gameConstraints.MaximalMovesPerPlayer)
				{
					WinnerAvailable?.Invoke(currentBoardState.TopPlayer.Player, WinningReason.ExceedanceOfMaxMoves);
				}

				var nextBottomPlayerBotMove = GetBottomPlayerBotMove();

				if (nextBottomPlayerBotMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextBottomPlayerBotMove))
				{
					WinnerAvailable?.Invoke(currentBoardState.TopPlayer.Player, WinningReason.InvalidMove);
					break;
				}								

				currentBoardState = currentBoardState.ApplyMove(nextBottomPlayerBotMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				if (nextBottomPlayerBotMove is Capitulation)
				{
					WinnerAvailable?.Invoke(currentBoardState.TopPlayer.Player, WinningReason.Capitulation);
				}

				var winner = GameAnalysis.CheckWinningCondition(currentBoardState);
				if (winner != null)
				{
					WinnerAvailable?.Invoke(winner, WinningReason.RegularQuoridorWin);
					break;
				}								
				
				var nextTopPlayerBotMove = GetTopPlayerBotMove();

				if (nextTopPlayerBotMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextTopPlayerBotMove))
				{
					WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player, WinningReason.InvalidMove);
					break;
				}

				currentBoardState = currentBoardState.ApplyMove(nextTopPlayerBotMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				if (nextTopPlayerBotMove is Capitulation)
				{
					WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player, WinningReason.Capitulation);
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

			bottomPlayerBot.NextMoveAvailable -= OnNextBottomPlayerBotMoveAvailable;
			topPlayerBot.NextMoveAvailable    -= OnNextTopPlayerBotMoveAvailable;
		}

		private Move GetBottomPlayerBotMove()
		{
			bottomPlayerBotMoves.Clear();
			bottomPlayerBotTimer.Change(gameConstraints.MaximalComputingTimePerMove, TimeSpan.Zero);
			bottomPlayerBot.DoMove(currentBoardState);			

			Move nextMove;
			while ((nextMove = bottomPlayerBotMoves.TimeoutTake()) == null)
			{
				if (stopRunning)
					return null;
			}

			if (nextMove is BotsTimeOut)
			{				
				WinnerAvailable?.Invoke(currentBoardState.TopPlayer.Player, WinningReason.ExceedanceOfThoughtTime);
				return null;
			}
				
			return nextMove;
		}

		private Move GetTopPlayerBotMove ()
		{
			topPlayerBotMoves.Clear();
			topPlayerBotTimer.Change(gameConstraints.MaximalComputingTimePerMove, TimeSpan.Zero);
			topPlayerBot.DoMove(currentBoardState);

			Move nextMove;
			while ((nextMove = topPlayerBotMoves.TimeoutTake()) == null)
			{
				if (stopRunning)
					return null;
			}

			if (nextMove is BotsTimeOut)
			{
				WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player, WinningReason.ExceedanceOfThoughtTime);
				return null;
			}

			return nextMove;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}