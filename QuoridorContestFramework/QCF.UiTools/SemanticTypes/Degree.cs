using QCF.UiTools.SemanticTypes.Base;

namespace QCF.UiTools.SemanticTypes
{
	public class Degree : SimpleDoubleSemanticType
    {        
        public Degree(double value)
            : base(value, "deg")
        {            
        }        
    }
}
