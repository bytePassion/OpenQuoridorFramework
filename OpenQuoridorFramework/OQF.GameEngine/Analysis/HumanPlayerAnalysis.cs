using System.Collections.Generic;
using System.Linq;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.GameEngine.Contracts;

namespace OQF.GameEngine.Analysis
{
	internal class HumanPlayerAnalysis : IHumanPlayerAnalysis
	{
		private readonly IList<FieldCoordinate> possibleMoves;
		private readonly IList<Wall>            possibleWalls;

		public HumanPlayerAnalysis(BoardState boardState)
		{		
			var gameGraph = new Graph(boardState);
							
			possibleMoves = new List<FieldCoordinate>();
			possibleWalls = new List<Wall>();

			if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
			{
				var node = gameGraph.GetNode(boardState.BottomPlayer.Position);

				foreach (var nodeNeighbor in node.Neighbours)
				{
					possibleMoves.Add(nodeNeighbor.Coord);
				}

				var allWalls = GeneratePotensialPossibleWalls(boardState);

				foreach (var wall in allWalls)
				{
					// TODO: hier lieber nur die offensichtlichen ausschließen
//					if (gameGraph.ValidateWallMove(new WallMove(null, null, wall)))
//					{
//						possibleWalls.Add(wall);
//					}
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
