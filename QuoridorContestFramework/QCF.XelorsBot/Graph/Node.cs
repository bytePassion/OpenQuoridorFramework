namespace QCF.XelorsBot.Graph
{
	internal class Node
	{
		public Node() { }

		private Node(Node left, Node top, Node right, Node bottom)
		{			
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		
		public Node Left;
		public Node Top;
		public Node Right;
		public Node Bottom;

		public Node Clone()
		{
			return new Node(Left, Top, Right, Bottom);
		}	
	}
}
