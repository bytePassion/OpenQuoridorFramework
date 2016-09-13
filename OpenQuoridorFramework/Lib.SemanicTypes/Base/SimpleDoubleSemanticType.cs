using System;
using Lib.Utils;

namespace Lib.SemanicTypes.Base
{
	public abstract class SimpleDoubleSemanticType : SemanticType<double>
    {        
        protected SimpleDoubleSemanticType(double value, string unit = "")
            : base(value, unit)
        {            
        }
        
        protected override Func<SemanticType<double>, SemanticType<double>, bool> EqualsFunc
        {
            get { return (st1, st2) => GeometryLibUtils.DoubleEquals(st1.Value, st2.Value); }
        }
        
        protected override string StringRepresentation => $"{GeometryLibUtils.DoubleFormat(Value)}";        

        public static implicit operator double(SimpleDoubleSemanticType doubleType)
        {
            return doubleType.Value;
        }
    }
}
