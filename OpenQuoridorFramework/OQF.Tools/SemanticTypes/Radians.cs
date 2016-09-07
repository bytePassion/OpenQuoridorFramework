using OQF.Tools.SemanticTypes.Base;

namespace OQF.Tools.SemanticTypes
{

	public class Radians : SimpleDoubleSemanticType
    {
        public Radians(double value)
            : base(value, "rad")
        {            
        }       
    }
}