using System;
using System.Threading;
using Lib.Concurrency;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Games;

namespace OQF.GameEngine.Game
{
	public class LocalGamePvB : IPvBGame
	{
		public event Action<Player, WinningReason, Move> WinnerAvailable;
		public event Action<BoardState>                  NextBoardstateAvailable;
		public event Action<string>                      DebugMessageAvailable;

		/*
		 *	computer is topPlayer
		 *	human is bottomPlayer
		 * 
		 */

		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly GameLoopThreadPvB gameLoopThreadPvB;
		private readonly IQuoridorBot quoridorBot;
		
		internal LocalGamePvB(IQuoridorBot unInitializedBot, string botName, GameConstraints gameConstraints, string initialProgress)
		{
			quoridorBot = unInitializedBot; 			
			quoridorBot.DebugMessageAvailable += OnDebugMessageAvailable;

			humenMoves = new TimeoutBlockingQueue<Move>(200);
			gameLoopThreadPvB = new GameLoopThreadPvB(quoridorBot, botName, humenMoves, gameConstraints, initialProgress);

			gameLoopThreadPvB.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThreadPvB.WinnerAvailable        += OnWinnerAvailable;

			new Thread(gameLoopThreadPvB.Run).Start();
		}

		private void OnDebugMessageAvailable(string s)
		{
			DebugMessageAvailable?.Invoke(s);
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move invalidMove)
		{
			StopGame();
			WinnerAvailable?.Invoke(player, winningReason, invalidMove);
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
			quoridorBot.DebugMessageAvailable -= OnDebugMessageAvailable;

			gameLoopThreadPvB.Stop();

			gameLoopThreadPvB.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			gameLoopThreadPvB.WinnerAvailable        -= OnWinnerAvailable;
		}
	}
}