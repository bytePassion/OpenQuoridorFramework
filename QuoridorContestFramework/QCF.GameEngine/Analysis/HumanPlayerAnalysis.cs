using System.Collections.Generic;
using System.Linq;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;
using QCF.GameEngine.Contracts;

namespace QCF.GameEngine.Analysis
{
	internal class HumanPlayerAnalysis : IHumanPlayerAnalysis
	{
		private readonly IList<FieldCoordinate> possibleMoves;
		private readonly IList<Wall>            possibleWalls;

		public HumanPlayerAnalysis(BoardState boardState)
		{			
			var gameGraph = new GameGraph().InitGraph()
										   .ApplyWallsAndPlayers(boardState);

			possibleMoves = new List<FieldCoordinate>();
			possibleWalls = new List<Wall>();

			if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
			{
				var node = gameGraph.GetNodeForCoordinate(boardState.BottomPlayer.Position);

				foreach (var nodeNeighbor in node.Neighbors)
				{
					possibleMoves.Add(nodeNeighbor.Coordinate);
				}

				var allWalls = GeneratePotensialPossibleWalls(boardState);

				foreach (var wall in allWalls)
				{
					// TODO: hier lieber nur die offensichtlichen ausschließen
					if (gameGraph.ValidateWallMove(new WallMove(null, null, wall)))
					{
						possibleWalls.Add(wall);
					}
				}
			}							
		}

		public IEnumerable<Wall> GetPossibleWalls()
		{
			// TODO: implementation
			return new List<Wall>();
		}

		public IEnumerable<FieldCoordinate> GetPossibleMoves() => possibleMoves;


		private static IList<Wall> GeneratePotensialPossibleWalls (BoardState boardState)
		{
			var resultList = new List<Wall>(128);

			for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
			{
				for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
				{
					var coord = new FieldCoordinate(xCoord, yCoord);

					if (boardState.PlacedWalls.Any(wall => wall.TopLeft == coord))

					resultList.Add(new Wall(coord, WallOrientation.Horizontal));
					resultList.Add(new Wall(coord, WallOrientation.Vertical));
				}
			}

			return resultList;
		}
	}
}
