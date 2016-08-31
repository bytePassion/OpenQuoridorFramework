using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCF.Contest.Contracts.Coordination;

namespace QCF.GameEngine.Analysis
{
    public static class InitialGraphCreator
    {
        public static GameGraph CreateInitialGameGraph(GameGraph graph)
        {

            foreach (var xField in Enum.GetValues(typeof(XField)))
            {
                var rowNodes = CreateRowNodes((XField)xField);
                foreach (var rowNode in rowNodes)
                {
                    graph.Graph.Add(rowNode);
                }
            }

            for (var i = 0; i < graph.Graph.Count; i++)
            {
                
            }
            return graph;
        }

        private static IEnumerable<Node> CreateRowNodes(XField xField)
        {
            foreach (var yField in Enum.GetValues(typeof(YField)))
            {
                yield return new Node(new FieldCoordinate(xField, (YField)yField));
            }
        }
    }
}
