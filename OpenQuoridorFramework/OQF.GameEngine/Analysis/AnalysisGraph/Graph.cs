using System;
using System.Collections.Generic;
using System.Linq;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.GameEngine.Analysis.AnalysisGraph
{
	public class Graph
	{
		private readonly FieldCoordinate bottomPlayerPosition;
		private readonly FieldCoordinate topPlayerPosition;

		private Node removedEdgeEndPoint1;
		private Node removedEdgeEndPoint2;

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

		private bool TraverseGraph (Node startNode, YField target)
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

		private bool ValidateFigureMove (FigureMove move, PlayerType playerType)
		{
			var sourceCoordinate = playerType == PlayerType.BottomPlayer
										? bottomPlayerPosition
										: topPlayerPosition;

			var targetCoordinate = move.NewPosition;

			var sourceNode = GetNode(sourceCoordinate);
			var targetNode = GetNode(targetCoordinate);

			return sourceNode.Neighbours.Contains(targetNode);
		}

		private bool IsWallApplyable(WallMove wallMove)
		{
			var wall = wallMove.PlacedWall;

			var topleft     = wall.TopLeft;
			var bottomLeft  = topleft.GetBottom();
			var topRight    = topleft.GetRight();
			var bottomRight = topRight.GetBottom();

			if (wall.Orientation == WallOrientation.Horizontal)
			{
				return Nodes[topleft].Bottom  != null &&
				       Nodes[bottomLeft].Top  != null &&
					   Nodes[topRight].Bottom != null &&
				       Nodes[bottomRight].Top != null;
			}
			else
			{
				return Nodes[topleft].Right    != null &&
					   Nodes[bottomLeft].Right != null &&
					   Nodes[topRight].Left    != null &&
					   Nodes[bottomRight].Left != null;
			}			
		}

		private bool ValidateWallMove (WallMove wallMove)
		{
			UndoSpecialEdges();

			if (!IsWallApplyable(wallMove))
				return false;

			ApplyWall(wallMove.PlacedWall);

			AddSpecialEdges();

			return TraverseGraph(GetNode(topPlayerPosition),    YField.One) &&
				   TraverseGraph(GetNode(bottomPlayerPosition), YField.Nine);
		}
		
		public bool ValidateMove (Move move, PlayerType playerType)
		{
			if (move is FigureMove)
				return ValidateFigureMove((FigureMove)move, playerType);

			if (move is WallMove)
				return ValidateWallMove((WallMove)move);

			throw new ArgumentException();
		}

		private void RemoveEdge (Node node1, Node node2)
		{
			node1.RemoveEdge(node2);
			node2.RemoveEdge(node1);
		}

		private void AddEdge(Node node1, Node node2)
		{
			node1.AddEdgeToNode(node2);
			node2.AddEdgeToNode(node1);
		}

		private void AddSpecialEdges ()
		{
			var topNode    = GetNode(topPlayerPosition);
			var bottomNode = GetNode(bottomPlayerPosition);

			if (PlayersAreNeighbours())
			{				
				RemoveEdge(topNode, bottomNode);

				removedEdgeEndPoint1 = topNode;
				removedEdgeEndPoint2 = bottomNode;

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

		private void UndoSpecialEdges ()
		{
			foreach (var nodePair in Nodes)
			{
				nodePair.Value.RemoveAllSpecialEdges();
			}

			if (removedEdgeEndPoint1 != null && removedEdgeEndPoint2 != null)
			{
				AddEdge(removedEdgeEndPoint1, removedEdgeEndPoint2);

				removedEdgeEndPoint1 = null;
				removedEdgeEndPoint2 = null;
			}
		}
	}
}
