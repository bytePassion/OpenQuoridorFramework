using System;
using Lib.Concurrency;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.GameLogic
{
	public interface IGameLoopThread : IThread
	{
		event Action<BoardState> NewBoardStateAvailable;
		event Action<Player, WinningReason, Move> WinnerAvailable;
	}
}