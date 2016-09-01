using System.Diagnostics;
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

            var gameGraph = new GameGraph().InitGraph().ApplyWalls(currentBoardState.PlacedWalls);
		    if (potentialNextMove.GetType() == typeof(FigureMove))
		    {
		        FieldCoordinate currentPosition;
		        if (currentBoardState.CurrentMover.PlayerType == PlayerType.TopPlayer)
		        {
		            currentPosition = currentBoardState.TopPlayer.Position;
		        }
		        else
		        {
		            currentPosition = currentBoardState.BottomPlayer.Position;
		        }
                Debug.WriteLine(gameGraph.ToString());
                return gameGraph.ValidateMove(currentPosition, ((FigureMove) potentialNextMove).NewPosition,
		            currentBoardState.CurrentMover);

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
