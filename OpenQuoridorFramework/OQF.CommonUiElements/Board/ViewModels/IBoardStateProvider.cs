using System;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.ViewModels
{
	public interface IBoardStateProvider
	{
		event Action<BoardState> NewBoardStateAvailable;

		BoardState CurrentBoardState { get; }
	}
}