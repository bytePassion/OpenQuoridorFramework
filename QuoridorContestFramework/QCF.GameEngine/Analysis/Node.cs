using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;

namespace QCF.GameEngine.Analysis
{
	public class Node
    {
        public FieldCoordinate Coordinate { get; }
        public List<Node> Neighbors { get; } = new List<Node>();
        public bool Visited { get; set; }

        public Node(FieldCoordinate coord)
        {
            Coordinate = coord;
        }

        public void AddNeighbors(IEnumerable<Node> neighbors)
        {
	        Neighbors.AddRange(neighbors);	        
        }

        public override string ToString()
        {
            return Coordinate.ToString();
        }
    }
}
