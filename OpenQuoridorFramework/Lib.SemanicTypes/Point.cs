using Lib.SemanicTypes.Base;

namespace Lib.SemanicTypes
{

	public class Point : TupleDoubleSemanticType<XCoord, YCoord>
    {
        public static readonly Point Zero = new Point(new XCoord(0), new YCoord(0));

        public Point(XCoord width, YCoord height)
            : base(width, height)
        {
        }
        
        public XCoord XCoord => Value1;
        public YCoord YCoord => Value2;
       
        public static implicit operator System.Windows.Point(Point size)
        {
            return new System.Windows.Point(size.Value1, size.Value2);
        }

        public static explicit operator Point(System.Windows.Point point)
        {
            return new Point(new XCoord(point.X), new YCoord(point.Y));
        }
    }
}