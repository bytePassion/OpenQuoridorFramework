using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCF.GameEngine.Contracts.Coordination
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
