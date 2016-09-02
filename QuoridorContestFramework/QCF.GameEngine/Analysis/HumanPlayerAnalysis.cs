using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.GameEngine.Contracts;

namespace QCF.GameEngine.Analysis
{
	internal class HumanPlayerAnalysis : IHumanPlayerAnalysis
	{
		private readonly BoardState boardState;

		private readonly IEnumerable<FieldCoordinate> possibleMoves;

		public HumanPlayerAnalysis(BoardState boardState)
		{
			this.boardState = boardState;

			var gameGraph = new GameGraph().InitGraph()
										   .ApplyWalls(boardState.PlacedWalls);

			possibleMoves = new List<FieldCoordinate>();

			if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
			{
				var node = gameGraph.GetNodeForCoordinate(boardState.BottomPlayer.Position);

				foreach (var nodeNeighbor in node.Neighbors)
				{
					((List<FieldCoordinate>)possibleMoves).Add(nodeNeighbor.Coordinate);
				}
			}			
		}

		public IEnumerable<Wall> GetPossibleWalls()
		{
			// TODO: implementation
			return new List<Wall>();
		}

		public IEnumerable<FieldCoordinate> GetPossibleMoves() => possibleMoves;
	}
}
