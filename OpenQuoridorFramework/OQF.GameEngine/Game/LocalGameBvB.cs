using System;
using System.Threading;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Games;
using OQF.GameEngine.Transitions;

namespace OQF.GameEngine.Game
{
	public class LocalGameBvB : IBvBGame
	{
		public event Action<Player, WinningReason> WinnerAvailable;
		public event Action<BoardState>            NextBoardstateAvailable;
		public event Action<PlayerType, string>    DebugMessageAvailable;
		
		private readonly GameLoopThreadBvB gameLoopThreadBvB;

		private readonly IQuoridorBot topPlayerBot;
		private readonly IQuoridorBot bottomPlayerBot;
		
		internal LocalGameBvB(IQuoridorBot uninitializedTopPlayerBot, 
							  IQuoridorBot uninitializedBottomPlayerBot, 
							  GameConstraints gameConstraints)
		{ 
			var topPlayer    = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);
		
			topPlayerBot = uninitializedTopPlayerBot;
			topPlayerBot.Init(topPlayer.PlayerType, gameConstraints);
			topPlayerBot.DebugMessageAvailable += OnTopPlayerBotDebugMessageAvailable;

			bottomPlayerBot = uninitializedBottomPlayerBot;
			bottomPlayerBot.Init(bottomPlayer.PlayerType, gameConstraints);
			bottomPlayerBot.DebugMessageAvailable += OnBottomPlayerBotDebugMessageAvailable;		
			
			var initialBoadState = BoardStateTransition.CreateInitialBoadState(topPlayer, bottomPlayer);
			
			gameLoopThreadBvB = new GameLoopThreadBvB(bottomPlayerBot, topPlayerBot, initialBoadState, gameConstraints);

			gameLoopThreadBvB.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameLoopThreadBvB.WinnerAvailable        += OnWinnerAvailable;

			new Thread(gameLoopThreadBvB.Run).Start();
		}

		private void OnTopPlayerBotDebugMessageAvailable(string s)
		{
			DebugMessageAvailable?.Invoke(PlayerType.TopPlayer, s);
		}

		private void OnBottomPlayerBotDebugMessageAvailable (string s)
		{
			DebugMessageAvailable?.Invoke(PlayerType.BottomPlayer, s);
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

		public void StopGame()
		{
			topPlayerBot.DebugMessageAvailable    -= OnTopPlayerBotDebugMessageAvailable;			
			bottomPlayerBot.DebugMessageAvailable -= OnBottomPlayerBotDebugMessageAvailable;

			gameLoopThreadBvB.Stop();

			gameLoopThreadBvB.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			gameLoopThreadBvB.WinnerAvailable        -= OnWinnerAvailable;
		}
	}
}