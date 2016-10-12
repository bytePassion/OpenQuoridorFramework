using System;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;

namespace OQF.PlayerVsBot.Visualization.Services
{
	public interface IGameService
	{
		event Action<BoardState>                  NewBoardStateAvailable;
		event Action<string>                      NewDebugMsgAvailable;
		event Action<Player, WinningReason, Move> WinnerAvailable;

		BoardState CurrentBoardState { get; }		
		
		void CreateGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, string initialProgress = null);
		void ReportHumanMove(Move move);
		void StopGame();
	}
}