using QCF.UiTools.SemanticTypes.Base;

namespace QCF.UiTools.SemanticTypes
{

	public class Radians : SimpleDoubleSemanticType
    {
        public Radians(double value)
            : base(value, "rad")
        {            
        }       
    }
}