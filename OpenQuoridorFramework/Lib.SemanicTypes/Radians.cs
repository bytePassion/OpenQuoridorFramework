using Lib.SemanicTypes.Base;

namespace Lib.SemanicTypes
{

	public class Radians : SimpleDoubleSemanticType
    {
        public Radians(double value)
            : base(value, "rad")
        {            
        }       
    }
}