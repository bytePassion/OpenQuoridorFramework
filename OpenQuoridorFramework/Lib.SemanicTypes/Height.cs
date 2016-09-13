using Lib.SemanicTypes.Base;

namespace Lib.SemanicTypes
{

	public class Height : SimpleDoubleSemanticType
    {
        public Height(double value)
            : base(value) 
        {
        }

        public static Height operator +(Height h1, Height h2) => new Height(h1.Value + h2.Value);
        public static Height operator -(Height h1, Height h2) => new Height(h1.Value - h2.Value);
        public static Height operator *(Height h1, Height h2) => new Height(h1.Value * h2.Value);
        public static Height operator /(Height h1, Height h2) => new Height(h1.Value / h2.Value);
        public static Height operator -(Height h)             => new Height(-h.Value);
    }
}