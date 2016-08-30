using System;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;
using QCF.UiTools.ConcurrencyLib;

namespace QCF.GameEngine
{
	public class GameLoopThread : IThread
	{
		public event Action<BoardState> NewBoardStateAvailable;
		public event Action<Player>     WinnerAvailable;

		private readonly IQuoridorBot bot;
		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly TimeoutBlockingQueue<Move> botMoves;

		private volatile bool stopRunning;

		private BoardState currentBoardState;
		
		public GameLoopThread (IQuoridorBot bot, 
							   TimeoutBlockingQueue<Move> humenMoves, 
							   BoardState initialBoardState)
		{
			this.bot = bot;
			this.humenMoves = humenMoves;

			botMoves = new TimeoutBlockingQueue<Move>(300);

			bot.NextMoveAvailable += OnNextBotMoveAvailable;

			currentBoardState = initialBoardState;

			stopRunning = false;
			IsRunning = false;
		}

		private void OnNextBotMoveAvailable(Move move)
		{
			botMoves.Put(move);
		}

		public void Run ()
		{
			IsRunning = true;			

			NewBoardStateAvailable?.Invoke(currentBoardState);			

			while (!stopRunning)
			{
				var nextHumanMove = PickHumanMove();

				if (nextHumanMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextHumanMove))
				{
					WinnerAvailable?.Invoke(currentBoardState.TopPlayer.Player);
					break;
				}								

				currentBoardState = currentBoardState.ApplyMove(nextHumanMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				var winner = GameAnalysis.CheckWinningCondition(currentBoardState);
				if (winner != null)
				{
					WinnerAvailable?.Invoke(winner);
					break;
				}
				
				
				var nextBotMove = GetBotMove();

				if (nextBotMove == null)
					break;

				if (!GameAnalysis.IsMoveLegal(currentBoardState, nextBotMove))
				{
					WinnerAvailable?.Invoke(currentBoardState.BottomPlayer.Player);
					break;
				}

				currentBoardState = currentBoardState.ApplyMove(nextBotMove);
				NewBoardStateAvailable?.Invoke(currentBoardState);

				var winner2 = GameAnalysis.CheckWinningCondition(currentBoardState);
				if (winner2 != null)
				{
					WinnerAvailable?.Invoke(winner2);
					break;
				}				
			}

			IsRunning = false;
			bot.NextMoveAvailable -= OnNextBotMoveAvailable;
		}

		private Move GetBotMove()
		{
			botMoves.Clear();
			bot.DoMove(currentBoardState);

			Move nextMove;
			while ((nextMove = botMoves.TimeoutTake()) == null)
			{
				if (stopRunning)
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

			return nextMove;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}