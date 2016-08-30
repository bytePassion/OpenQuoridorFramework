using System;
using System.Reflection;
using System.Threading;
using QCF.Contest.Contracts;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Loader;
using QCF.GameEngine.Transitions;
using QCF.Tools.ConcurrencyLib;

namespace QCF.GameEngine.Game
{
	public class LocalGamePvC : IGame
	{
		public event Action<Player>     WinnerAvailable;
		public event Action<BoardState> NextBoardstateAvailable;
		public event Action<string>     DebugMessageAvailable;

		/*
		 *	computer is topPlayer
		 *	human is bottomPlayer
		 * 
		 */

		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly GameLoopThread gameLoopThread;
		private readonly IQuoridorBot quoridorAi;
		
		internal LocalGamePvC(string botDllFile)
		{
			var computerPlayer = new Player(PlayerType.TopPlayer);
			var humanPlayer = new Player(PlayerType.BottomPlayer);

			humenMoves = new TimeoutBlockingQueue<Move>(1000);
			
			quoridorAi = new BotLoader().LoadBot(Assembly.LoadFile(botDllFile));
			quoridorAi.Init(computerPlayer);
			quoridorAi.DebugMessageAvailable += OnDebugMessageAvailable;
			
			var initialBoadState = BoardStateTransition.CreateInitialBoadState(computerPlayer, humanPlayer);
			
			gameLoopThread = new GameLoopThread(quoridorAi, humenMoves, initialBoadState);

			gameLoopThread.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThread.WinnerAvailable        += OnWinnerAvailable;

			new Thread(gameLoopThread.Run).Start();
		}

		private void OnDebugMessageAvailable(string s)
		{
			DebugMessageAvailable?.Invoke(s);
		}

		private void OnWinnerAvailable(Player player)
		{
			StopGame();
			WinnerAvailable?.Invoke(player);
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			NextBoardstateAvailable?.Invoke(boardState);
		}

		public void ReportHumanMove(Move move)
		{
			humenMoves.Put(move);
		}

		public void StopGame()
		{
			quoridorAi.DebugMessageAvailable -= OnDebugMessageAvailable;

			gameLoopThread.Stop();

			gameLoopThread.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			gameLoopThread.WinnerAvailable        -= OnWinnerAvailable;
		}
	}
}