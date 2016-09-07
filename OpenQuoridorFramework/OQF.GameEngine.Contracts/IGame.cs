using System;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace OQF.GameEngine.Contracts
{
	public interface IGame
	{
		event Action<Player, WinningReason> WinnerAvailable;
		event Action<BoardState> NextBoardstateAvailable;
		event Action<string> DebugMessageAvailable;

		void ReportHumanMove (Move move);

		void StopGame();
	}	
}