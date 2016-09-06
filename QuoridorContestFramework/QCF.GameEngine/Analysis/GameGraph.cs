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

        public void RemoveNode(FieldCoordinate coordinate)
        {
	        var nodeToRemove = GetNodeForCoordinate(coordinate);
	        Graph.Remove(nodeToRemove);          
        }

        public GameGraph ApplyWallsAndPlayers(BoardState boardState)
        {
            foreach (var wall in boardState.PlacedWalls)
            {
               ApplyWall(wall);
            }

	        bottomPlayerPosition = boardState.BottomPlayer.Position;
	        topPlayerPosition = boardState.TopPlayer.Position;

			// TODO: addSpezialEdges if players are neighbours

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

	    public bool ValidateWallMove(WallMove wallMove)
	    {				
			ApplyWall(wallMove.PlacedWall);

			// TODO: Check for spezial edges
					
			return TraverseGraph(GetNodeForCoordinate(topPlayerPosition),    endCoordinatesForTopPlayer) &&
				   TraverseGraph(GetNodeForCoordinate(bottomPlayerPosition), endCoordinatesForBottomPlayer);			
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
			else if (move is WallMove)
				return ValidateWallMove((WallMove) move);
			else			
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