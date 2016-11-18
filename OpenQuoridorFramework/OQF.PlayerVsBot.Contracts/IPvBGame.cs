using System;
using OQF.AnalysisAndProgress.Enum;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.PlayerVsBot.Contracts
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