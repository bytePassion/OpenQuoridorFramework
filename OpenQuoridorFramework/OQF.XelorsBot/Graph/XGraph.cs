using System.Collections.Generic;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;

namespace OQF.XelorsBot.Graph
{
	internal class XGraph
	{
		public XGraph()
		{
			Nodes = Construction.GetAllNodes();
			Construction.AddAllEdges(Nodes);
		}

		private XGraph(IDictionary<FieldCoordinate, Node> nodes)
		{
			Nodes = nodes;
		}

		IDictionary<FieldCoordinate, Node> Nodes;

		public void ApplyWall(Wall wall)
		{
			var topleft     = wall.TopLeft;
			var bottomLeft  = topleft.GetBottom();
			var topRight    = topleft.GetRight();
			var bottomRight = topRight.GetBottom();

			if (wall.Orientation == WallOrientation.Horizontal)
			{				
				Nodes[topleft    ].Bottom = null;
				Nodes[bottomLeft ].Top    = null;
				Nodes[topRight   ].Bottom = null;
				Nodes[bottomRight].Top    = null;
			}
			else
			{
				Nodes[topleft    ].Right = null;
				Nodes[bottomLeft ].Left  = null;
				Nodes[topRight   ].Right = null;
				Nodes[bottomRight].Left  = null;
			}
		}

		public XGraph Clone()
		{
			IDictionary<FieldCoordinate, Node> nodes = new Dictionary<FieldCoordinate, Node>();

			foreach (var pair in Nodes)
			{
				nodes.Add(pair.Key, pair.Value.Clone());
			}

			return new XGraph(nodes);
		}
	}
}
