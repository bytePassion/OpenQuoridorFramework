using System;
using System.Collections.Generic;
using System.Linq;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace OQF.GameEngine.Analysis
{
	public class Graph
	{
		private readonly FieldCoordinate bottomPlayerPosition;
		private readonly FieldCoordinate topPlayerPosition;

		public Graph(BoardState boardState = null)
		{
			Nodes = GraphConstruction.GetAllNodes();
			GraphConstruction.AddAllEdges(Nodes);			

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

		public IDictionary<FieldCoordinate, Node> Nodes { get; }

		public Node GetNode (FieldCoordinate coordinate)
		{
			return Nodes[coordinate];
		}

		private void ApplyWall(Wall wall)
		{
			var topleft     = wall.TopLeft;
			var bottomLeft  = topleft.GetBottom();
			var topRight    = topleft.GetRight();
			var bottomRight = topRight.GetBottom();

			if (wall.Orientation == WallOrientation.Horizontal)
			{
				Nodes[topleft    ].RemoveEdge(Nodes[topleft    ].Bottom);
				Nodes[bottomLeft ].RemoveEdge(Nodes[bottomLeft ].Top);
				Nodes[topRight   ].RemoveEdge(Nodes[topRight   ].Bottom);
				Nodes[bottomRight].RemoveEdge(Nodes[bottomRight].Top);				
			}
			else
			{
				Nodes[topleft    ].RemoveEdge(Nodes[topleft    ].Right);
				Nodes[bottomLeft ].RemoveEdge(Nodes[bottomLeft ].Right);
				Nodes[topRight   ].RemoveEdge(Nodes[topRight   ].Left);
				Nodes[bottomRight].RemoveEdge(Nodes[bottomRight].Left);		
			}
		}

		private bool TraverseGraph2 (Node startNode, YField target)
		{			
			var stack = new Stack<Node>();
			stack.Push(startNode);
			startNode.Visited = true;

			while (stack.Count != 0)
			{
				var nextNode = stack.Peek();
				if (nextNode.Coord.YCoord == target)
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
			return false;
		}


		private void ClearVisitStatusForNodes ()
		{
			foreach (var nodePair in Nodes)
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

			return TraverseGraph2(GetNode(topPlayerPosition),    YField.One) &&
				   TraverseGraph2(GetNode(bottomPlayerPosition), YField.Nine);
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
			if (targetNode == null || !Nodes.ContainsKey(targetNode.Coord))
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
			foreach (var nodePair in Nodes)
			{
				nodePair.Value.RemoveAllSpecialEdges();
			}
		}
	}
}
