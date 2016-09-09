using SemanicTypesLib.Base;

namespace SemanicTypesLib
{
	public class Degree : SimpleDoubleSemanticType
    {        
        public Degree(double value)
            : base(value, "deg")
        {            
        }        
    }
}
