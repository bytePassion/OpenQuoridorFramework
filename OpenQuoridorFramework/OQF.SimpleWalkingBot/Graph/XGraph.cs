using System;
using System.Collections.Generic;
using System.Linq;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;

namespace OQF.SimpleWalkingBot.Graph
{
	internal class XGraph
	{
		private readonly FieldCoordinate playerPosition;
		private readonly FieldCoordinate oppenentPosition;

		public XGraph(BoardState currentBoardState, PlayerType playerType)
		{
			nodes = Construction.GetAllNodes();
			Construction.AddAllEdges(nodes);

			foreach (var wall in currentBoardState.PlacedWalls)			
				ApplyWall(wall);

			playerPosition = playerType == PlayerType.BottomPlayer
									? currentBoardState.BottomPlayer.Position
									: currentBoardState.TopPlayer.Position;

			oppenentPosition = playerType == PlayerType.TopPlayer
									? currentBoardState.BottomPlayer.Position
									: currentBoardState.TopPlayer.Position;
		}		
		
		private readonly IDictionary<FieldCoordinate, Node> nodes;

		private void ApplyWall(Wall wall)
		{
			var topleft     = wall.TopLeft;
			var bottomLeft  = topleft.GetBottom();
			var topRight    = topleft.GetRight();
			var bottomRight = topRight.GetBottom();

			if (wall.Orientation == WallOrientation.Horizontal)
			{				
				nodes[topleft    ].Bottom = null;
				nodes[bottomLeft ].Top    = null;
				nodes[topRight   ].Bottom = null;
				nodes[bottomRight].Top    = null;
			}
			else
			{
				nodes[topleft    ].Right = null;
				nodes[bottomLeft ].Right = null;
				nodes[topRight   ].Left  = null;
				nodes[bottomRight].Left  = null;
			}
		}

		private static void TraverseGraph (Node node, int cost)
		{
			if (node == null)
				return;

			if (node.MinConst == -1)
			{
				node.MinConst = cost;
			}
			else
			{
				if (node.MinConst <= cost)
					return;
				else				
					node.MinConst = cost;
				
			}
			
			TraverseGraph(node.Left,   cost+1);
			TraverseGraph(node.Top,    cost+1);
			TraverseGraph(node.Bottom, cost+1);
			TraverseGraph(node.Right,  cost+1);
		}


		private void ClearVisitStatusForNodes ()
		{
			foreach (var nodePair in nodes)
				nodePair.Value.MinConst = -1;
		}

		private Node GetTargetField(YField target)
		{
			var targets = nodes.Where(fieldPair => fieldPair.Key.YCoord == target);

			var minCostNode = targets.First().Value;

			foreach (var keyValuePair in targets)
			{
				if (minCostNode.MinConst == -1 || keyValuePair.Value.MinConst < minCostNode.MinConst)
					minCostNode = keyValuePair.Value;
			}

			return minCostNode;			
		}

		private static IList<Node> GetReversePath(IList<Node> currentList, Node currentNode)
		{
			currentList.Add(currentNode);

			if (currentNode.MinConst == 1)
				return currentList;

			var costDictionary = new Dictionary<int, Node>();

			if (currentNode.Left   != null)                                                              costDictionary.Add(currentNode.Left.MinConst,   currentNode.Left);
			if (currentNode.Top    != null) if(!costDictionary.ContainsKey(currentNode.Top.MinConst))    costDictionary.Add(currentNode.Top.MinConst,    currentNode.Top);
			if (currentNode.Bottom != null) if(!costDictionary.ContainsKey(currentNode.Bottom.MinConst)) costDictionary.Add(currentNode.Bottom.MinConst, currentNode.Bottom);
			if (currentNode.Right  != null) if(!costDictionary.ContainsKey(currentNode.Right.MinConst))  costDictionary.Add(currentNode.Right.MinConst,  currentNode.Right);

			var min = costDictionary.Min(pair => pair.Key);

			return GetReversePath(currentList, costDictionary[min]);
		}		
		
		private IEnumerable<Node> GetReversePath(Node startNode, YField target)
		{
			ClearVisitStatusForNodes();
			TraverseGraph(startNode, 0);
			var nearestTarget = GetTargetField(target);
			var reversePath = GetReversePath(new List<Node>(), nearestTarget);
			return reversePath;
		}

		public FieldCoordinate GetNextPositionToMove(YField target)
		{
			var playerNode = nodes[playerPosition];
			var opponendNode = nodes[oppenentPosition];

			var reversePath = GetReversePath(nodes[playerPosition], target);
			var nextField = reversePath.Last().Coord;

			if (nextField != oppenentPosition)
			{
				return nextField;
			}
			else
			{
				if (playerNode.Left   != null && playerNode.Left   != opponendNode) return playerNode.Left.Coord;
				if (playerNode.Top    != null && playerNode.Top    != opponendNode) return playerNode.Top.Coord;
				if (playerNode.Bottom != null && playerNode.Bottom != opponendNode) return playerNode.Bottom.Coord;
				if (playerNode.Right  != null && playerNode.Right  != opponendNode) return playerNode.Right.Coord;
			}

			throw new Exception();
		}		
	}
}
