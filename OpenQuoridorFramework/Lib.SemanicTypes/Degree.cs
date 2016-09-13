using Lib.SemanicTypes.Base;

namespace Lib.SemanicTypes
{
	public class Degree : SimpleDoubleSemanticType
    {        
        public Degree(double value)
            : base(value, "deg")
        {            
        }        
    }
}
