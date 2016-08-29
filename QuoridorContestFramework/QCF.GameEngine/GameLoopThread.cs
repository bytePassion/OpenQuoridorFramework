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
		
		public GameLoopThread (IQuoridorBot bot, TimeoutBlockingQueue<Move> humenMoves, BoardState initialBoardState)
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

				// TODO: check if move is legal
				// TODO: check if winner

				currentBoardState = currentBoardState.ApplyMove(nextHumanMove);

				NewBoardStateAvailable?.Invoke(currentBoardState);


				var nextBotMove = GetBotMove();

				if (nextBotMove == null)
					break;

				// TODO: check if move is legel
				// TODO: check if winner

				currentBoardState = currentBoardState.ApplyMove(nextBotMove);
			}

			IsRunning = false;
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