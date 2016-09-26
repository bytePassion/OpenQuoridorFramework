using System;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Enums;

namespace OQF.GameEngine.Contracts.Games
{
	public interface IBvBGame
	{
		event Action<Player, WinningReason> WinnerAvailable;
		event Action<BoardState> NextBoardstateAvailable;
		event Action<PlayerType, string> DebugMessageAvailable;		

		void StopGame ();
	}
}