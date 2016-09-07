using System;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace OQF.Contest.Contracts
{
	public interface IQuoridorBot
	{
		/// <summary>
		/// Fire this event when you want to submit your next move.
		/// </summary>
		event Action<Move> NextMoveAvailable;

	    /// <summary>
	    /// Fire this event to send a DebugMessage to the UI
	    /// </summary>
	    event Action<string> DebugMessageAvailable;
         
		/// <summary>
		/// This method is called before the game starts. You will be informed 
		/// whether you are the bottom- or top player. You can choose a name
		/// by setting the name property.
		/// </summary>				
		void Init (Player yourPlayer);

		/// <summary>
		/// This method is called when it's your turn. To finish your turn
		/// fire the NextMoveAvailable-event to report your next move.
		/// </summary>
		/// <param name="currentState">current situation of the board</param>
		void DoMove(BoardState currentState);
	}
}
