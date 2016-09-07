using OQF.Tools.SemanticTypes.Base;

namespace OQF.Tools.SemanticTypes
{
	public class Degree : SimpleDoubleSemanticType
    {        
        public Degree(double value)
            : base(value, "deg")
        {            
        }        
    }
}
