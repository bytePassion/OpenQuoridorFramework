using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.GameEngine.Analysis
{
	//non directed graph with edges added in both directions 
	public class GameGraph
    {
        private readonly List<FieldCoordinate> endCoordinatesForBottomPlayer = new List<FieldCoordinate>();
        private readonly List<FieldCoordinate> endCoordinatesForTopPlayer    = new List<FieldCoordinate>();

        private IList<Tuple<Node, Node>> specialEdgesToAdd; 
        private IList<Tuple<Node, Node>> RemovedSpecialEdges; 

		public IList<Node> Graph { get; } = new List<Node>();
		public IDictionary<FieldCoordinate, Node> GraphDictionary { get; } = new Dictionary<FieldCoordinate, Node>();

	    private FieldCoordinate bottomPlayerPosition;
	    private FieldCoordinate topPlayerPosition;
		
        public GameGraph()
        {
            InitEndCoordinates();
        }

        public GameGraph InitGraph()
        {
            return InitialGraphCreator.CreateInitialGameGraph(this);
        }

        private void InitEndCoordinates()
        {
	        for (var xCoord = XField.A; xCoord <= XField.I; xCoord++)
	        {
				endCoordinatesForBottomPlayer.Add(new FieldCoordinate(xCoord, YField.Nine));
				endCoordinatesForTopPlayer.Add(new FieldCoordinate(xCoord, YField.One));
			}            
        }

        public Node GetNodeForCoordinate(FieldCoordinate coordinate)
        {
            return GraphDictionary[coordinate];
        }

        public void RemoveEdge(Node node1, Node node2)
        {
            node1.Neighbors.Remove(node2);
            node2.Neighbors.Remove(node1);
        }

        public GameGraph ApplyWallsAndPlayers(BoardState boardState)
        {
            foreach (var wall in boardState.PlacedWalls)
            {
               ApplyWall(wall);
            }

	        bottomPlayerPosition = boardState.BottomPlayer.Position;
	        topPlayerPosition = boardState.TopPlayer.Position;

            AddSpecialEdges();

            return this;
        }

	    private void ApplyWall(Wall wall)
	    {
			var topleftNode     = GetNodeForCoordinate(wall.TopLeft);
			var bottomleftNode  = GetNodeForCoordinate(wall.TopLeft.GetBottom());
			var topRightNode    = GetNodeForCoordinate(wall.TopLeft.GetRight());
			var bottomRightNode = GetNodeForCoordinate(wall.TopLeft.GetBottom().GetRight());

			if (wall.Orientation == WallOrientation.Horizontal)
			{
				topleftNode.Neighbors.Remove(bottomleftNode);
				topRightNode.Neighbors.Remove(bottomRightNode);
				bottomleftNode.Neighbors.Remove(topleftNode);
				bottomRightNode.Neighbors.Remove(topRightNode);
			}
			else
			{
				topleftNode.Neighbors.Remove(topRightNode);
				topRightNode.Neighbors.Remove(topleftNode);
				bottomleftNode.Neighbors.Remove(bottomRightNode);
				bottomRightNode.Neighbors.Remove(bottomleftNode);
			}
		}

        private bool TraverseGraph(Node startNode, IEnumerable<FieldCoordinate> targetCoords)
        {
            foreach (var targetCoord in targetCoords)
            {
                var stack = new Stack<Node>();
                stack.Push(startNode);
                startNode.Visited = true;

                while (stack.Count != 0)
                {
                    var nextNode = stack.Peek();
                    if (nextNode.Coordinate.Equals(targetCoord))
                    {
                        ClearVisitStatusForNodes();
                        return true;
                    }

                    var child = nextNode.Neighbors.FirstOrDefault(n => !n.Visited);
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

        private void ClearVisitStatusForNodes()
        {
            foreach (var node in Graph)
                node.Visited = false;
        }

	    private bool ValidateWallMove(WallMove wallMove)
	    {	
            RemoveSpecialEdges();			
			ApplyWall(wallMove.PlacedWall);

            AddSpecialEdges();
            					
			return TraverseGraph(GetNodeForCoordinate(topPlayerPosition),    endCoordinatesForTopPlayer) &&
				   TraverseGraph(GetNodeForCoordinate(bottomPlayerPosition), endCoordinatesForBottomPlayer);			
	    }

	    private void AddSpecialEdges()
	    {
            specialEdgesToAdd = new List<Tuple<Node, Node>>();
            RemovedSpecialEdges = new List<Tuple<Node, Node>>();

            var topNode = GetNodeForCoordinate(topPlayerPosition);
            var bottomNode = GetNodeForCoordinate(bottomPlayerPosition);

            if (PlayersAreNeighbours())
	        {
                RemovedSpecialEdges.Add(new Tuple<Node, Node>(topNode, bottomNode));
                RemoveEdge(topNode, bottomNode);

                ComputeSpecialEdges(topNode, bottomNode);
	            ComputeSpecialEdges(bottomNode, topNode);
	        }
            
	    }

	    private bool IsMoveable(Node sourceNode, Node targetNode)
	    {
	        if (!GraphDictionary.ContainsKey(targetNode.Coordinate))
	            return false;

	        return sourceNode.Neighbors.Contains(targetNode);
	    }

	    private void ComputeSpecialEdges(Node playerNode, Node opponentPlayerNode)
	    {

	        if (playerNode.Coordinate.XCoord == opponentPlayerNode.Coordinate.XCoord)
	        {
	            if (playerNode.Coordinate.YCoord < opponentPlayerNode.Coordinate.YCoord)
	            {
	                if (IsMoveable(opponentPlayerNode, GetNodeForCoordinate(playerNode.Coordinate.GetBottom())))
	                {
	                    
	                }
	            }
	            else
	            {
	                
	            }
	        }
	        else
	        {
	            if (playerNode.Coordinate.XCoord < opponentPlayerNode.Coordinate.XCoord)
	            {

	            }
	            else
	            {
	                
	            }
	        }
	    }

	    private bool PlayersAreNeighbours()
	    {
	        return GetNodeForCoordinate(topPlayerPosition).Neighbors.Contains(GetNodeForCoordinate(bottomPlayerPosition));
	    }

	    private void RemoveSpecialEdges()
	    {
	        
	    }

	    private bool ValidateFigureMove(FigureMove move)
	    {
			var sourceCoordinate = move.PlayerAtMove.PlayerType == PlayerType.BottomPlayer 
										? bottomPlayerPosition 
										: topPlayerPosition;

		    var targetCoordinate = move.NewPosition;

		    var sourceNode = GetNodeForCoordinate(sourceCoordinate);
		    var targetNode = GetNodeForCoordinate(targetCoordinate);

			return sourceNode.Neighbors.Contains(targetNode);
	    }

	    public bool ValidateMove(Move move)
	    {
			if (move is FigureMove)
				return ValidateFigureMove((FigureMove)move);

			if (move is WallMove)
				return ValidateWallMove((WallMove) move);
					
			throw new ArgumentException();					    
	    }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in Graph)
            {
                var elements = "";
                node.Neighbors.ForEach(value => elements += value + ",");
                builder.AppendLine($"{node.Coordinate}: {elements}");
            }

            return builder.ToString();
        }
    }
}