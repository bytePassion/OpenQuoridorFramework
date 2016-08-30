using QCF.Contest.Contracts.Coordination;

namespace QCF.GameEngine
{
	public class Edge
    {
        public FieldCoordinate Node1 { get; }
        public FieldCoordinate Node2 { get; }

        public Edge(FieldCoordinate node1, FieldCoordinate node2)
        {
            Node1 = node1;
            Node2 = node2;
        }
    }
}
