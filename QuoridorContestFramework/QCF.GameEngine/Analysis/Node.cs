using System.Collections.Generic;
using System.Linq;
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
            this.Coordinate = coord;
        }

        public Node AddNeighbors(IEnumerable<Node> neighbors)
        {
            Neighbors.AddRange(neighbors);
            return this;
        }

        public override string ToString()
        {
            return Coordinate.ToString();
        }
    }
}
