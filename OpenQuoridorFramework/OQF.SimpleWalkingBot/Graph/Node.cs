using OQF.Contest.Contracts.Coordination;

namespace OQF.SimpleWalkingBot.Graph
{
	internal class Node
	{

		public Node (FieldCoordinate coord)
		{
			Coord = coord;
		}

		public readonly FieldCoordinate Coord;

		public int MinConst;
			
		public Node Left;
		public Node Top;
		public Node Right;
		public Node Bottom;	
	}
}
