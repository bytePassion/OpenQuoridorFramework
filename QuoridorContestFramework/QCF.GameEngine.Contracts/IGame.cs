using System;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.GameEngine.Contracts
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