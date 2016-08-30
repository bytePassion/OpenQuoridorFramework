using System;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.SingleGameVisualization.Services
{
	internal interface IGameService
	{
		event Action<BoardState> NewBoardStateAvailable;
		event Action<string> NewDebugMsgAvailable;
		event Action<Player> WinnerAvailable;

		BoardState CurrentBoardState { get; }

		void CreateGame(string dllPath);
		void ReportHumanMove(Move move);
		void StopGame();
	}
}