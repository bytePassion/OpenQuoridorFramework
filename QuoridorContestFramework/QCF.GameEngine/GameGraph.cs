using System.Collections.Generic;
using System.Text;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;

namespace QCF.GameEngine
{
	//non directed graph with edges added in both directions 
	public class GameGraph
    {
        private readonly Dictionary<FieldCoordinate, List<FieldCoordinate>> graph = new Dictionary<FieldCoordinate, List<FieldCoordinate>>();

        private void AddEdge(Edge edge)
        {
            AddNode(edge.Node1);
            AddNode(edge.Node2);

            AddNeighbor(edge);
        }

        private void AddNeighbor(Edge edge)
        {
            if(!graph[edge.Node1].Contains(edge.Node2))
                graph[edge.Node1].Add(edge.Node2);

            if (!graph[edge.Node2].Contains(edge.Node1))
                graph[edge.Node2].Add(edge.Node1);
        }

        private void AddNode(FieldCoordinate node)
        {
            if(!graph.ContainsKey(node))
                graph.Add(node, new List<FieldCoordinate>());
        }

        public void RemoveEdge(Edge edge)
        {
            
        }

        public bool ValidateMove(Edge edge, Player player)
        {
            //nodes of edge still connected?
            if (!graph[edge.Node1].Contains(edge.Node2))
                return false;
            if (!IsExitPathAvailable(edge.Node1, player))
                return false;
            //search if open path still available

            return true;
        }

        private bool IsExitPathAvailable(FieldCoordinate startCoordinatem, Player player)
        {
            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in graph)
            {
                var elements = "";
                node.Value.ForEach(value => elements += value + ",");
                builder.AppendLine($"{node.Key}: {elements}");
            }

            return builder.ToString();
        }
    }
}
