using System.Diagnostics;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.GameEngine.Analysis
{
	internal static class GameAnalysis
	{
	    private static GameGraph Graph { get; } = new GameGraph().InitGraph();

	    public static bool IsMoveLegal(BoardState currentBoardState, Move potentialNextMove)
		{

		    if (potentialNextMove.GetType() == typeof(FigureMove))
		    {
		        var move = (FigureMove)potentialNextMove;
		        //var node1 = ((FigureMove) move.StateBeforeMove.LastMove).NewPosition;
		        //var node2 = move.NewPosition;
		        Debug.WriteLine(Graph.ToString());
		    }
		    else
		    {
		        
		    }

            return true;
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
