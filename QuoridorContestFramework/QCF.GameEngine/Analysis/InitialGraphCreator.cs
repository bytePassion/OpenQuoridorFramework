using System;
using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;

namespace QCF.GameEngine.Analysis
{
	public static class InitialGraphCreator
    {
        public static GameGraph CreateInitialGameGraph(GameGraph graph)
        {
            foreach (var xField in Enum.GetValues(typeof(XField)))
            {
                var rowNodes = CreateRowNodes((XField) xField);
	            foreach (var rowNode in rowNodes)
	            {
		            graph.Graph.Add(rowNode);
					graph.GraphDictionary.Add(rowNode.Coordinate, rowNode);
	            }
            }

            InitNeighbors(graph);
            return graph;
        }

        private static void InitNeighbors(GameGraph graph)
        {
            //corner neighbors
            graph.Graph[0].AddNeighbors(new List<Node> {graph.Graph[9], graph.Graph[1]});
            graph.Graph[8].AddNeighbors(new List<Node> {graph.Graph[9], graph.Graph[1]});
            graph.Graph[72].AddNeighbors(new List<Node> {graph.Graph[63], graph.Graph[73]});
            graph.Graph[80].AddNeighbors(new List<Node> {graph.Graph[79], graph.Graph[71]});

            //top bottom neighbors

            for (var i = 1; i < 8; i++)
                graph.Graph[i].AddNeighbors(new List<Node> {graph.Graph[i - 1], graph.Graph[i + 9], graph.Graph[i + 1]});

            for (var i = 73; i < 80; i++)
                graph.Graph[i].AddNeighbors(new List<Node> {graph.Graph[i - 1], graph.Graph[i - 9], graph.Graph[i + 1]});

            for (var i = 9; i < 72; i++)
                if (i%9 == 0)
                    graph.Graph[i].AddNeighbors(new List<Node>
	                    {
		                    graph.Graph[i - 9],
		                    graph.Graph[i + 1],
		                    graph.Graph[i + 9]
	                    });
                else if (i%9 == 8)
                    graph.Graph[i].AddNeighbors(new List<Node>
	                    {
		                    graph.Graph[i - 9],
		                    graph.Graph[i - 1],
		                    graph.Graph[i + 9]
	                    });
                else
                    graph.Graph[i].AddNeighbors(new List<Node>
	                    {
		                    graph.Graph[i - 1],
		                    graph.Graph[i - 9],
		                    graph.Graph[i + 1],
		                    graph.Graph[i + 9]
	                    });
        }

        private static IEnumerable<Node> CreateRowNodes(XField xField)
        {
            foreach (var yField in Enum.GetValues(typeof(YField)))
                yield return new Node(new FieldCoordinate(xField, (YField) yField));
        }
    }
}