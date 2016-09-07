using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace OQF.GameEngine.Analysis
{
	internal static class GameAnalysis
	{
	    public static bool IsMoveLegal(BoardState currentBoardState, Move potentialNextMove)
		{
		    if (potentialNextMove is Capitulation)
		    {
			    return true;
		    }

			var gameGraph = new Graph(currentBoardState);

			return gameGraph.ValidateMove(potentialNextMove);           		              
		}

		public static Player CheckWinningCondition(BoardState currentBoardState)
		{
			var topPlayerState = currentBoardState.TopPlayer;

			if (topPlayerState.Position.YCoord == YField.One)
				return topPlayerState.Player;

			var bottomPlayerState = currentBoardState.BottomPlayer;

			if (bottomPlayerState.Position.YCoord == YField.Nine)
				return bottomPlayerState.Player;

			return null;
		}
	}
}
