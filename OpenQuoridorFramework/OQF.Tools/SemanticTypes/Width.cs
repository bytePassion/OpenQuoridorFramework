﻿using OQF.Tools.SemanticTypes.Base;

namespace OQF.Tools.SemanticTypes
{

	public class Width : SimpleDoubleSemanticType
    {
        public Width(double value)
            : base(value)
        {
        }

        public static Width operator +(Width w1, Width w2) => new Width(w1.Value + w2.Value);
        public static Width operator -(Width w1, Width w2) => new Width(w1.Value - w2.Value);
        public static Width operator *(Width w1, Width w2) => new Width(w1.Value * w2.Value);
        public static Width operator /(Width w1, Width w2) => new Width(w1.Value / w2.Value);
        public static Width operator -(Width w)            => new Width(-w.Value);
    }
}