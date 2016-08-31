using QCF.Tools.SemanticTypes.Base;

namespace QCF.Tools.SemanticTypes
{

	public class Radians : SimpleDoubleSemanticType
    {
        public Radians(double value)
            : base(value, "rad")
        {            
        }       
    }
}