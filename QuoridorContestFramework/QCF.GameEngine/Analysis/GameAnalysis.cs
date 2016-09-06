using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.GameEngine.Analysis
{
	internal static class GameAnalysis
	{
	    public static bool IsMoveLegal(BoardState currentBoardState, Move potentialNextMove)
		{
		    if (potentialNextMove is Capitulation)
		    {
			    return true;
		    }

            return new GameGraph().InitGraph()
								  .ApplyWallsAndPlayers(currentBoardState)
								  .ValidateMove(potentialNextMove);
		              
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
