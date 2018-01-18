using System.Linq;
using OQF.AnalysisAndProgress.Analysis.AnalysisGraph;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.AnalysisAndProgress.Analysis
{
    public static class GameAnalysis
    {
        public static bool IsMoveLegal(BoardState currentBoardState, Move potentialNextMove)
        {
            if (potentialNextMove is Capitulation)
            {
                return true;
            }

            if (potentialNextMove is WallMove move)
            {
                if (currentBoardState.PlacedWalls.Select(wall => wall.TopLeft).Contains(move.PlacedWall.TopLeft))
                {
                    return false;
                }

                if (currentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
                {
                    if (currentBoardState.BottomPlayer.WallsToPlace == 0)
                        return false;					
                }
                else
                {
                    if (currentBoardState.TopPlayer.WallsToPlace == 0)
                        return false;
                }
            }

            var gameGraph = new Graph(currentBoardState);

            return gameGraph.ValidateMove(potentialNextMove, currentBoardState.CurrentMover.PlayerType);           		              
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
