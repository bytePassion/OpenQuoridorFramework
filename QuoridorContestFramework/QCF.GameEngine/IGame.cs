using System;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;

namespace QCF.GameEngine
{
	public interface IGame
	{
		event Action<Player> WinnerAvailable;
		event Action<BoardState> NextBoardstateAvailable;
		event Action<string> DebugMessageAvailable;

		void ReportHumanMove (Move move);

		void StopGame();
	}	
}