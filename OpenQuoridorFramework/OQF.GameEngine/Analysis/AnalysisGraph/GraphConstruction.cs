using System.Collections.Generic;
using OQF.Bot.Contracts.Coordination;

namespace OQF.GameEngine.Analysis.AnalysisGraph
{
	internal static class GraphConstruction
	{
		public static IDictionary<FieldCoordinate, Node> GetAllNodes()
		{
			var result = new Dictionary<FieldCoordinate, Node>(81);

			for (var x = XField.A; x <= XField.I; x++)
			{
				for (var y = YField.One; y >= YField.Nine; y--)
				{
					var coord = new FieldCoordinate(x, y);
					result.Add(coord, new Node(coord));
				}
			}
			
			return result;		
		}

		public static void AddAllEdges(IDictionary<FieldCoordinate, Node> nodes)
		{
			foreach (var pair in nodes)
			{
				var fieldCoord = pair.Key;
				var node = pair.Value;

				if (fieldCoord.ExistsLeft())   { node.AddEdgeToNode(nodes[fieldCoord.GetLeft()]);   }
				if (fieldCoord.ExistsRight())  { node.AddEdgeToNode(nodes[fieldCoord.GetRight()]);  }
				if (fieldCoord.ExistsTop())    { node.AddEdgeToNode(nodes[fieldCoord.GetTop()]);    }
				if (fieldCoord.ExistsBottom()) { node.AddEdgeToNode(nodes[fieldCoord.GetBottom()]); }
			}
		}

		
	}
}
