using System.Collections.Generic;
using OQF.Bot.Contracts.Coordination;

namespace OQF.GameEngine.Analysis.AnalysisGraph
{
	public class Node
	{
		private readonly IList<Node> neighbours;

		public Node (FieldCoordinate coord)
		{
			Coord = coord;
			neighbours = new List<Node>();
		}

		public bool Visited { get; set; }

		public FieldCoordinate Coord { get; }

		public Node Left     { get; private set; }
		public Node Top      { get; private set; }
		public Node Right    { get; private set; }
		public Node Bottom   { get; private set; }
		public Node Special1 { get; private set; }
		public Node Special2 { get; private set; }

		public IEnumerable<Node> Neighbours => neighbours;
		

		public void AddEdgeToNode(Node nextNode)
		{
			switch (GetDirection(Coord, nextNode.Coord))
			{
				case Direction.Bottom: { Bottom = nextNode; break; }
				case Direction.Left:   { Left   = nextNode; break; }
				case Direction.Top:    { Top    = nextNode; break; }
				case Direction.Right:  { Right  = nextNode; break; }
			}

			neighbours.Add(nextNode);
		}

		public void RemoveEdge(Node nextNode)
		{
			switch (GetDirection(Coord, nextNode.Coord))
			{
				case Direction.Bottom: { Bottom = null; break; }
				case Direction.Left:   { Left   = null; break; }
				case Direction.Top:    { Top    = null; break; }
				case Direction.Right:  { Right  = null; break; }
			}

			neighbours.Remove(nextNode);
		}

		public void AddSpecialEdge(Node nextNode)
		{
			if (Special1 == null)
				Special1 = nextNode;
			else
				Special2 = nextNode;

			neighbours.Add(nextNode);
		}

		public void RemoveAllSpecialEdges()
		{
			if (Special1 != null)
			{
				neighbours.Remove(Special1);
				Special1 = null;
			}

			if (Special2 != null)
			{
				neighbours.Remove(Special2);
				Special2 = null;
			}			
		}

		private static Direction GetDirection(FieldCoordinate from, FieldCoordinate to)
		{
			if (from.XCoord == to.XCoord)
			{
				return from.YCoord > to.YCoord 
					? Direction.Top 
					: Direction.Bottom;
			}
			else
			{
				return from.XCoord < to.XCoord 
					? Direction.Right 
					: Direction.Left;
			}
		}

//		public override string ToString()
//		{
//			return $"{Coord}|Left({LeftToString()})Top({TopToString()})Right({RightToString()})Bottom({BottomToString()})";
//		}
//
//		private string LeftToString()    =>   Left?.Coord.ToString() ?? "--";
//		private string RightToString ()  =>  Right?.Coord.ToString() ?? "--";
//		private string TopToString ()    =>    Top?.Coord.ToString() ?? "--";
//		private string BottomToString () => Bottom?.Coord.ToString() ?? "--";
	}
}
