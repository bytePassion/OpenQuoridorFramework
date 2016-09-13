using System;
using Lib.FrameworkExtension;

namespace Lib.SemanicTypes.Base
{

	public abstract class SemanticType<T1, T2>
    {
        protected SemanticType(T1 value1, T2 value2, string unit1 = "", string unit2 = "")
        {
            Value1 = value1;
            Value2 = value2;
            Unit1 = unit1;
            Unit2 = unit2;
        } 

        public T1 Value1 { get; }
        public T2 Value2 { get; }

        protected string Unit1 { get; }
        protected string Unit2 { get; }

        protected abstract Func<SemanticType<T1, T2>, SemanticType<T1,T2>, bool> EqualsFunc { get; }

        protected abstract string StringRepresentation1 { get; }
        protected abstract string StringRepresentation2 { get; }

        public override bool   Equals(object obj) => this.Equals(obj, (st1, st2) => EqualsFunc(st1, st2));
        public override int    GetHashCode()      => Value1.GetHashCode() ^ Value2.GetHashCode();
        public override string ToString()         => $"({ToString1},{ToString2})";
        
        private string ToString1 => string.IsNullOrEmpty(Unit1) ? StringRepresentation1 : StringRepresentation1 + " " + Unit1;
        private string ToString2 => string.IsNullOrEmpty(Unit2) ? StringRepresentation2 : StringRepresentation2 + " " + Unit2;
        
        public static bool operator ==(SemanticType<T1, T2> st1, SemanticType<T1, T2> st2) => EqualsExtension.EqualsForEqualityOperator(st1, st2);
        public static bool operator !=(SemanticType<T1, T2> st1, SemanticType<T1, T2> st2) => !(st1 == st2);
    }

}