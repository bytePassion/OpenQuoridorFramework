﻿using System.Collections.Generic;
using System.Linq;
using OQF.AnalysisAndProgress.Analysis.AnalysisGraph;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.AnalysisAndProgress.Analysis
{
    public static class PlayerAnalysis 
    {		
        public static PlayerAnalysisResult GetResult(BoardState boardState, PlayerType forPlayerType)
        {		
            var gameGraph = new Graph(boardState);
                            
            if (boardState.CurrentMover.PlayerType == forPlayerType)
            {
                var possibleMoves = gameGraph.GetNode(forPlayerType == PlayerType.BottomPlayer 
                                                            ? boardState.BottomPlayer.Position 
                                                            : boardState.TopPlayer.Position)
                                             .Neighbours
                                             .Select(nodeNeigbour => nodeNeigbour.Coord);
                
                var possibleWalls = GeneratePhysicalPossibleWalls(boardState);

                return new PlayerAnalysisResult(possibleWalls, possibleMoves);
            }
            
            return null;
        }

        

//		First version
//		
//		private static IList<Wall> GeneratePhysicalPossibleWalls (BoardState boardState)
//		{
//			var resultList = new List<Wall>();
//
//			for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
//			{
//				for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
//				{
//					var coord = new FieldCoordinate(xCoord, yCoord);
//
//				    var placedWalls = boardState.PlacedWalls;
//
//					if (placedWalls.Any(wall => wall.TopLeft == coord))
//						continue;
//
//					if (!placedWalls.Any(wall => wall.Orientation == WallOrientation.Horizontal &&
//					                             (wall.TopLeft == new FieldCoordinate(coord.XCoord - 1, coord.YCoord) ||
//					                              wall.TopLeft == new FieldCoordinate(coord.XCoord + 1, coord.YCoord))))
//					{
//						resultList.Add(new Wall(coord, WallOrientation.Horizontal));
//					}
//
//					if (!placedWalls.Any(wall => wall.Orientation == WallOrientation.Vertical &&
//												 (wall.TopLeft == new FieldCoordinate(coord.XCoord, coord.YCoord + 1) ||
//												  wall.TopLeft == new FieldCoordinate(coord.XCoord, coord.YCoord - 1))))
//					{
//						resultList.Add(new Wall(coord, WallOrientation.Vertical));
//					}					
//				}
//			}
//
//			return resultList;
//		}

        private static IEnumerable<Wall> GeneratePhysicalPossibleWalls (BoardState boardState)
        {
            var allHorizontalWalls = new Dictionary<FieldCoordinate, Wall>();
            var allVerticalWalls  = new Dictionary<FieldCoordinate, Wall>();			

            for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
            {
                for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
                {
                    var coord = new FieldCoordinate(xCoord, yCoord);

                    allHorizontalWalls.Add(coord, new Wall(coord, WallOrientation.Horizontal));
                    allVerticalWalls.Add  (coord, new Wall(coord, WallOrientation.Vertical));					
                }
            }

            var horizontalWalls = boardState.PlacedWalls.Where(wall => wall.Orientation == WallOrientation.Horizontal);
            var verticalWalls   = boardState.PlacedWalls.Where(wall => wall.Orientation == WallOrientation.Vertical);

            foreach (var verticalWall in verticalWalls)
            {
                var topLeft = verticalWall.TopLeft;

                allHorizontalWalls.Remove(topLeft);
                allVerticalWalls.Remove(topLeft);				
                allVerticalWalls.Remove(new FieldCoordinate(topLeft.XCoord, topLeft.YCoord + 1));
                allVerticalWalls.Remove(new FieldCoordinate(topLeft.XCoord, topLeft.YCoord - 1));
            }

            foreach (var horizontalWall in horizontalWalls)
            {
                var topLeft = horizontalWall.TopLeft;

                allVerticalWalls.Remove(topLeft);
                allHorizontalWalls.Remove(topLeft);				
                allHorizontalWalls.Remove(new FieldCoordinate(topLeft.XCoord - 1, topLeft.YCoord));
                allHorizontalWalls.Remove(new FieldCoordinate(topLeft.XCoord + 1, topLeft.YCoord));
            }

            return allHorizontalWalls.Values.Concat(allVerticalWalls.Values);
        }
    }
}
