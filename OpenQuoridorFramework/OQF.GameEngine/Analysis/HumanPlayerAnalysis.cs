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

				possibleWalls = GeneratePotentialPossibleWalls(boardState);				
			}							
		}

		public IEnumerable<Wall>            GetPossibleWalls() => possibleWalls;		
		public IEnumerable<FieldCoordinate> GetPossibleMoves() => possibleMoves;

		private static IList<Wall> GeneratePotentialPossibleWalls (BoardState boardState)
		{
			var resultList = new List<Wall>();

			for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
			{
				for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
				{
					var coord = new FieldCoordinate(xCoord, yCoord);

					var placedWalls = boardState.PlacedWalls;

					if (placedWalls.Any(wall => wall.TopLeft == coord))
						continue;

					if (!placedWalls.Any(wall => wall.Orientation == WallOrientation.Horizontal &&
					                             (wall.TopLeft == new FieldCoordinate(coord.XCoord - 1, coord.YCoord) ||
					                              wall.TopLeft == new FieldCoordinate(coord.XCoord + 1, coord.YCoord))))
					{
						resultList.Add(new Wall(coord, WallOrientation.Horizontal));
					}

					if (!placedWalls.Any(wall => wall.Orientation == WallOrientation.Vertical &&
												 (wall.TopLeft == new FieldCoordinate(coord.XCoord, coord.YCoord + 1) ||
												  wall.TopLeft == new FieldCoordinate(coord.XCoord, coord.YCoord - 1))))
					{
						resultList.Add(new Wall(coord, WallOrientation.Vertical));
					}					
				}
			}

			return resultList;
		}
	}
}
