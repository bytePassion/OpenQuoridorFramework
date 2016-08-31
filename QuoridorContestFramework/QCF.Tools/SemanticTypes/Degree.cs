using QCF.Tools.SemanticTypes.Base;

namespace QCF.Tools.SemanticTypes
{
	public class Degree : SimpleDoubleSemanticType
    {        
        public Degree(double value)
            : base(value, "deg")
        {            
        }        
    }
}
