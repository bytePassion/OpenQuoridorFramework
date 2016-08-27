using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;
using QCF.UiTools.ConcurrencyLib;
using QFC.SimpleWalkingBot;

namespace QCF.GameEngine
{
	public class LocalGamePvC : IGamePvC
	{
		public event Action<Player> WinnerAvailable;
		public event Action<Move> NextMoveAvailable;
		public event Action<Player, string> DebugMessageAvailable;

		/*
		 *	computer is topPlayer
		 *	human is bottomPlayer
		 * 
		 */

		private readonly IQuoridorBot quoridorAi;

		private readonly TimeoutBlockingQueue<Move> humenMoves;

		private readonly Player computerPlayer;
		private readonly Player humanPlayer;

		private readonly GameLoopThread gameLoopThread;



		public LocalGamePvC(Player computerPlayer, Player humanPlayer)
		{
			this.computerPlayer = computerPlayer;
			this.humanPlayer = humanPlayer;

			humenMoves = new TimeoutBlockingQueue<Move>(1000);
			
			quoridorAi = new BotLoader().LoadBot(Assembly.LoadFile("QCF.SimpleWalkingBot.dll"));
			quoridorAi.Init(computerPlayer);

			var initialBoadState = BoardStateTransition.CreateInitialBoadState(computerPlayer, humanPlayer);

			gameLoopThread = new GameLoopThread(humenMoves, initialBoadState);
			new Thread(gameLoopThread.Run).Start();
		}
				
		public void ReportHumanMove(Move move)
		{
			humenMoves.Put(move);
		}
	}


	public class GameLoopThread : IThread
	{
		private readonly TimeoutBlockingQueue<Move> humenMoves; 

		private volatile bool stopRunning;

		private BoardState currentBoadState;
		
		public GameLoopThread (TimeoutBlockingQueue<Move> humenMoves, BoardState initialBoadState)
		{
			this.humenMoves = humenMoves;

			currentBoadState = initialBoadState;

			stopRunning = false;
			IsRunning = false;
		}

		public void Run ()
		{
			IsRunning = true;

			bool exit = false;

			

			while (!stopRunning)
			{

				Move nextHumanMove;

				while ((nextHumanMove = humenMoves.TimeoutTake()) == null)
				{
					if (stopRunning)
					{
						exit = true;
						break;
					}
				}

				if (exit)
					break;


			}
			
			IsRunning = false;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}