using System;
using QCF.GameEngine.Contracts.GameElements;
using QCF.GameEngine.Contracts.Moves;

namespace QCF.SingleGameVisualization.Services
{
	internal interface IGameService
	{
		event Action<BoardState> NewBoardStateAvailable;
		event Action<string> NewDebugMsgAvailable;
		event Action<Player> WinnerAvailable;


		void CreateGame(string dllPath);
		void ReportHumanMove(Move move);
		void StopGame();
	}
}