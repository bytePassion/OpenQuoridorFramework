using SemanicTypesLib.Base;

namespace SemanicTypesLib
{

	public class Radians : SimpleDoubleSemanticType
    {
        public Radians(double value)
            : base(value, "rad")
        {            
        }       
    }
}