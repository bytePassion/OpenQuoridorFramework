using OQF.Bot.Contracts.Coordination;

namespace SimpleWalkingBot.Graph
{
	internal class Node
	{

		public Node (FieldCoordinate coord)
		{
			Coord = coord;
			MinConst = -1;
		}

		public readonly FieldCoordinate Coord;

		public int MinConst;
			
		public Node Left;
		public Node Top;
		public Node Right;
		public Node Bottom;

		public override string ToString()
		{
			return $"{Coord}; {MinConst}";
		}
	}
}
