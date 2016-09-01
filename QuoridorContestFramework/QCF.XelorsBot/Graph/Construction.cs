﻿using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;

namespace QCF.XelorsBot.Graph
{
	internal static class Construction
	{
		public static IDictionary<FieldCoordinate, Node> GetAllNodes()
		{
			var result = new Dictionary<FieldCoordinate, Node>(81);

			for (var x = XField.A; x <= XField.I; x++)
			{
				for (var y = YField.One; y <= YField.Nine; y++)
				{					
					result.Add(new FieldCoordinate(x, y), new Node());
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

				if (fieldCoord.ExistsLeft())   { node.Left   = nodes[fieldCoord.GetLeft()  ]; }
				if (fieldCoord.ExistsRight())  { node.Right  = nodes[fieldCoord.GetRight() ]; }
				if (fieldCoord.ExistsTop())    { node.Top    = nodes[fieldCoord.GetTop()   ]; }
				if (fieldCoord.ExistsBottom()) { node.Bottom = nodes[fieldCoord.GetBottom()]; }
			}
		}

		
	}
}
