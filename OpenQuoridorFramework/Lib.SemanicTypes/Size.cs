using Lib.SemanicTypes.Base;

namespace Lib.SemanicTypes
{
	public class Size : TupleDoubleSemanticType<Width, Height>
    {
        public static readonly Size Zero = new Size(new Width(0), new Height(0));

        public Size(Width width, Height height)
            : base(width, height)
        {            
        }

        public Width  Width  => Value1;
        public Height Height => Value2;        

        public static implicit operator System.Windows.Size(Size size)
        {
            return new System.Windows.Size(size.Value1, size.Value2);
        }

        public static explicit operator Size(System.Windows.Size size)
        {
            return new Size(new Width(size.Width), new Height(size.Height));
        }
    }

}
