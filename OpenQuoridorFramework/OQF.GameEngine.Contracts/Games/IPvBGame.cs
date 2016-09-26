using System;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;

namespace OQF.GameEngine.Contracts.Games
{
	public interface IPvBGame
	{
		event Action<Player, WinningReason, Move> WinnerAvailable;
		event Action<BoardState> NextBoardstateAvailable;
		event Action<string> DebugMessageAvailable;

		void ReportHumanMove (Move move);
		
		void StopGame();
	}
}