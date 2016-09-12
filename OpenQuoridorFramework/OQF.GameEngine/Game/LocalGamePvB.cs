using System;
using System.Threading;
using ConcurrencyLib;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;
using OQF.GameEngine.Transitions;

namespace OQF.GameEngine.Game
{
	public class LocalGamePvB : IPvBGame
	{
		public event Action<Player, WinningReason> WinnerAvailable;
		public event Action<BoardState>            NextBoardstateAvailable;
		public event Action<string>                DebugMessageAvailable;

		/*
		 *	computer is topPlayer
		 *	human is bottomPlayer
		 * 
		 */

		private readonly TimeoutBlockingQueue<Move> humenMoves;
		private readonly GameLoopThreadPvB gameLoopThreadPvB;
		private readonly IQuoridorBot quoridorBot;
		
		internal LocalGamePvB(IQuoridorBot unInitializedBot, GameConstraints gameConstraints)
		{
			var computerPlayer = new Player(PlayerType.TopPlayer);
			var humanPlayer = new Player(PlayerType.BottomPlayer);

			humenMoves = new TimeoutBlockingQueue<Move>(200);
			
			quoridorBot = unInitializedBot; 
			quoridorBot.Init(computerPlayer, gameConstraints);
			quoridorBot.DebugMessageAvailable += OnDebugMessageAvailable;
			
			var initialBoadState = BoardStateTransition.CreateInitialBoadState(computerPlayer, humanPlayer);
			
			gameLoopThreadPvB = new GameLoopThreadPvB(quoridorBot, humenMoves, initialBoadState, gameConstraints);

			gameLoopThreadPvB.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThreadPvB.WinnerAvailable        += OnWinnerAvailable;

			new Thread(gameLoopThreadPvB.Run).Start();
		}

		private void OnDebugMessageAvailable(string s)
		{
			DebugMessageAvailable?.Invoke(s);
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason)
		{
			StopGame();
			WinnerAvailable?.Invoke(player, winningReason);
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