using QCF.Tools.SemanticTypes.Base;

namespace QCF.Tools.SemanticTypes
{

	public class XCoord : SimpleDoubleSemanticType
    {
        public XCoord(double value)
            : base(value)
        {
        }

        public static XCoord operator +(XCoord x1, XCoord x2) => new XCoord(x1.Value + x2.Value);
        public static XCoord operator -(XCoord x1, XCoord x2) => new XCoord(x1.Value - x2.Value);
        public static XCoord operator *(XCoord x1, XCoord x2) => new XCoord(x1.Value * x2.Value);
        public static XCoord operator /(XCoord x1, XCoord x2) => new XCoord(x1.Value / x2.Value);
        public static XCoord operator -(XCoord x)             => new XCoord(-x.Value);
    }
}