using System;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.BoardViewModel
{
	public interface IBoardStateProvider
	{
		event Action<BoardState> NewBoardStateAvailable;

		BoardState CurrentBoardState { get; }
	}
}