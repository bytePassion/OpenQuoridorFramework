using System;

namespace Lib.FrameworkExtension
{
	public static class EqualsExtension
	{
		public static bool Equals<T>(this T obj1, object obj2, Func<T, T, bool> compareFunc)
		{						
			if (obj2 == null) return false;

			if (obj1.GetType() != obj2.GetType()) return false;

			var objectAsType = (T)obj2;

			return compareFunc(obj1, objectAsType);
		}

		public static bool EqualsForEqualityOperator(object o1, object o2)
		{
			if (o1 == null)
				return o2 == null;

			return o1.Equals(o2);
		}
	}
}
