using System;
using System.Collections.Generic;
using System.Linq;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.GameEngine.Analysis
{
	internal class Graph
	{
		private readonly List<FieldCoordinate> endCoordinatesForBottomPlayer = new List<FieldCoordinate>();
		private readonly List<FieldCoordinate> endCoordinatesForTopPlayer    = new List<FieldCoordinate>();

		private readonly FieldCoordinate bottomPlayerPosition;
		private readonly FieldCoordinate topPlayerPosition;

		public Graph(BoardState boardState = null)
		{
			nodes = Construction.GetAllNodes();
			Construction.AddAllEdges(nodes);
			InitEndCoordinates();

			if (boardState != null)
			{
				foreach (var wall in boardState.PlacedWalls)
				{
					ApplyWall(wall);
				}

				bottomPlayerPosition = boardState.BottomPlayer.Position;
				topPlayerPosition = boardState.TopPlayer.Position;

				AddSpecialEdges();
			}
		}

		private void InitEndCoordinates ()
		{
			for (var xCoord = XField.A; xCoord <= XField.I; xCoord++)
			{
				endCoordinatesForBottomPlayer.Add(new FieldCoordinate(xCoord, YField.Nine));
				endCoordinatesForTopPlayer.Add(new FieldCoordinate(xCoord, YField.One));
			}
		}

		private readonly IDictionary<FieldCoordinate, Node> nodes;
		

		public Node GetNode (FieldCoordinate coordinate)
		{
			return nodes[coordinate];
		}
		

		public void ApplyWall(Wall wall)
		{
			var topleft     = wall.TopLeft;
			var bottomLeft  = topleft.GetBottom();
			var topRight    = topleft.GetRight();
			var bottomRight = topRight.GetBottom();

			if (wall.Orientation == WallOrientation.Horizontal)
			{
				nodes[topleft    ].RemoveEdge(nodes[topleft    ].Bottom);
				nodes[bottomLeft ].RemoveEdge(nodes[bottomLeft ].Top);
				nodes[topRight   ].RemoveEdge(nodes[topRight   ].Bottom);
				nodes[bottomRight].RemoveEdge(nodes[bottomRight].Top);				
			}
			else
			{
				nodes[topleft    ].RemoveEdge(nodes[topleft    ].Right);
				nodes[bottomLeft ].RemoveEdge(nodes[bottomLeft ].Right);
				nodes[topRight   ].RemoveEdge(nodes[topRight   ].Left);
				nodes[bottomRight].RemoveEdge(nodes[bottomRight].Left);		
			}
		}


		private bool TraverseGraph (Node startNode, IEnumerable<FieldCoordinate> targetCoords)
		{
			foreach (var targetCoord in targetCoords)
			{
				var stack = new Stack<Node>();
				stack.Push(startNode);
				startNode.Visited = true;

				while (stack.Count != 0)
				{
					var nextNode = stack.Peek();
					if (nextNode.Coord.Equals(targetCoord))
					{
						ClearVisitStatusForNodes();
						return true;
					}

					var child = nextNode.Neighbours.FirstOrDefault(n => !n.Visited);
					if (child == null)
					{
						stack.Pop();
					}
					else
					{
						child.Visited = true;
						stack.Push(child);
					}
				}

				ClearVisitStatusForNodes();
			}
			return false;
		}
	

		private void ClearVisitStatusForNodes ()
		{
			foreach (var nodePair in nodes)
				nodePair.Value.Visited = false;
		}

		private bool ValidateFigureMove (FigureMove move)
		{
			var sourceCoordinate = move.PlayerAtMove.PlayerType == PlayerType.BottomPlayer
										? bottomPlayerPosition
										: topPlayerPosition;

			var targetCoordinate = move.NewPosition;

			var sourceNode = GetNode(sourceCoordinate);
			var targetNode = GetNode(targetCoordinate);

			return sourceNode.Neighbours.Contains(targetNode);
		}

		private bool ValidateWallMove (WallMove wallMove)
		{
			RemoveSpecialEdges();
			ApplyWall(wallMove.PlacedWall);

			AddSpecialEdges();

			return TraverseGraph(GetNode(topPlayerPosition), endCoordinatesForTopPlayer) &&
				   TraverseGraph(GetNode(bottomPlayerPosition), endCoordinatesForBottomPlayer);
		}

		public bool ValidateMove (Move move)
		{
			if (move is FigureMove)
				return ValidateFigureMove((FigureMove)move);

			if (move is WallMove)
				return ValidateWallMove((WallMove)move);

			throw new ArgumentException();
		}

		private void RemoveEdge (Node node1, Node node2)
		{
			node1.RemoveEdge(node2);
			node2.RemoveEdge(node1);
		}

		private void AddSpecialEdges ()
		{
			var topNode    = GetNode(topPlayerPosition);
			var bottomNode = GetNode(bottomPlayerPosition);

			if (PlayersAreNeighbours())
			{				
				RemoveEdge(topNode, bottomNode);

				ComputeSpecialEdges(topNode, bottomNode);
				ComputeSpecialEdges(bottomNode, topNode);
			}

		}

		private bool IsMoveable (Node sourceNode, Node targetNode)
		{
			if (targetNode == null || !nodes.ContainsKey(targetNode.Coord))
				return false;

			return sourceNode.Neighbours.Contains(targetNode);
		}

		private void ComputeSpecialEdges (Node playerNode, Node opponentPlayerNode)
		{

			var overleapField = GetOverLeapField(playerNode, opponentPlayerNode);

			if (IsMoveable(opponentPlayerNode, overleapField))
			{
				playerNode.AddSpecialEdge(overleapField);
			}
			else
			{
				var diagonalLeftField = GetDiagonalLeftField(playerNode, opponentPlayerNode);

				if (IsMoveable(opponentPlayerNode, diagonalLeftField))
				{
					playerNode.AddSpecialEdge(diagonalLeftField);
				}

				var diagonalRightField = GetDiagonalRightField(playerNode, opponentPlayerNode);

				if (IsMoveable(opponentPlayerNode, diagonalRightField))
				{
					playerNode.AddSpecialEdge(diagonalRightField);
				}
			}
		}

		private static Node GetDiagonalLeftField(Node playerNode, Node opponentPlayerNode)
		{
			switch (GetDirection(playerNode.Coord, opponentPlayerNode.Coord))
			{
				case Direction.Bottom: { return opponentPlayerNode.Right;  }
				case Direction.Left:   { return opponentPlayerNode.Bottom; }
				case Direction.Top:    { return opponentPlayerNode.Left;   }
				case Direction.Right:  { return opponentPlayerNode.Top;    }
			}

			return null;
		}

		private static Node GetDiagonalRightField(Node playerNode, Node opponentPlayerNode)
		{
			switch (GetDirection(playerNode.Coord, opponentPlayerNode.Coord))
			{
				case Direction.Bottom: { return opponentPlayerNode.Left;   }
				case Direction.Left:   { return opponentPlayerNode.Top;    }
				case Direction.Top:    { return opponentPlayerNode.Right;  }
				case Direction.Right:  { return opponentPlayerNode.Bottom; }
			}

			return null;
		}

		private static Node GetOverLeapField(Node playerNode, Node opponentPlayerNode)
		{

			switch (GetDirection(playerNode.Coord, opponentPlayerNode.Coord))
			{
				case Direction.Bottom: { return opponentPlayerNode.Bottom; }
				case Direction.Left:   { return opponentPlayerNode.Left;   }
				case Direction.Top:    { return opponentPlayerNode.Top;    }
				case Direction.Right:  { return opponentPlayerNode.Right;  }
			}

			return null;
		}

		private static Direction GetDirection (FieldCoordinate from, FieldCoordinate to)
		{
			if (from.XCoord == to.XCoord)
			{
				return from.YCoord > to.YCoord
					? Direction.Top
					: Direction.Bottom;
			}

			return from.XCoord < to.XCoord
				? Direction.Right
				: Direction.Left;
		}

		private bool PlayersAreNeighbours ()
		{
			return GetNode(topPlayerPosition).Neighbours.Contains(GetNode(bottomPlayerPosition));
		}

		private void RemoveSpecialEdges ()
		{
			foreach (var nodePair in nodes)
			{
				nodePair.Value.RemoveAllSpecialEdges();
			}
		}
	}
}
