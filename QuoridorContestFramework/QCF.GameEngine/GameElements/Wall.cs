using QCF.GameEngine.Coordination;

namespace QCF.GameEngine.GameElements
{
	public class Wall
	{
		public Wall(XField left, XField right, YField top, YField bottom, WallOrientation orientation)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
			Orientation = orientation;
		}

		public XField Left   { get; }
		public XField Right  { get; }
		public YField Top    { get; }
		public YField Bottom { get; }

		public WallOrientation Orientation { get; }
	}
}
