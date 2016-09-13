using System;
using Lib.Utils;

namespace Lib.SemanicTypes.Base
{

	public abstract class TupleDoubleSemanticType<T1, T2> : SemanticType<T1, T2> 
        where T1 : SemanticType<double> 
        where T2 : SemanticType<double>
    {
        protected TupleDoubleSemanticType(T1 value1, T2 value2, string unit1 = "", string unit2 = "")
            : base(value1, value2, unit1, unit2)
        {            
        }
         
        protected override Func<SemanticType<T1, T2>, SemanticType<T1, T2>, bool> EqualsFunc            
        {
            get
            {
                return (st1, st2) => GeometryLibUtils.DoubleEquals(st1.Value1.Value, st2.Value1.Value) &&
                                     GeometryLibUtils.DoubleEquals(st1.Value2.Value, st2.Value2.Value);
            }
        }

        protected override string StringRepresentation1 => GeometryLibUtils.DoubleFormat(Value1.Value);
        protected override string StringRepresentation2 => GeometryLibUtils.DoubleFormat(Value2.Value);
    }

}