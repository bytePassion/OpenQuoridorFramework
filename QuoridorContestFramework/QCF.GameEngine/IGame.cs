using System;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;

namespace QCF.GameEngine
{
	public interface IGame
	{
		event Action<Player> WinnerAvailable;
		event Action<Move> NextMoveAvailable;
		event Action<Player, string> DebugMessageAvailable;
	}

	public interface IGamePvC : IGame
	{
		void ReportHumanMove(Move move);
	}
}