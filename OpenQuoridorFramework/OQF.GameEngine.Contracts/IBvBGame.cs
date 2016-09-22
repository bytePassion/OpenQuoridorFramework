using System;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IBvBGame
	{
		event Action<Player, WinningReason> WinnerAvailable;
		event Action<BoardState> NextBoardstateAvailable;
		event Action<PlayerType, string> DebugMessageAvailable;		

		void StopGame ();
	}
}