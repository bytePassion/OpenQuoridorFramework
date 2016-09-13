using System;
using Lib.FrameworkExtension;

namespace Lib.SemanicTypes.Base
{

	public abstract class SemanticType<T>
    {
        protected SemanticType(T value, string unit = "")
        {
            Value = value;
            Unit = unit;
        }

        public T Value { get; }
        protected string Unit { get; }

        protected abstract Func<SemanticType<T>,SemanticType<T>,bool> EqualsFunc { get; }
        protected abstract string StringRepresentation { get; }
        
        public override bool   Equals(object obj) => this.Equals(obj, (st1, st2) => EqualsFunc(st1, st2));
        public override int    GetHashCode()      => Value.GetHashCode();
        public override string ToString()         => string.IsNullOrEmpty(Unit) ? StringRepresentation : StringRepresentation + " " + Unit;
        
        public static bool operator ==(SemanticType<T> st1, SemanticType<T> st2) => EqualsExtension.EqualsForEqualityOperator(st1, st2);
        public static bool operator !=(SemanticType<T> st1, SemanticType<T> st2) => !(st1 == st2);
    }

}