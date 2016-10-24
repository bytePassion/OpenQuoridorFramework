using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ProgressCodingTest.Coding
{
	internal static class Base64Coding
	{		
		private const string CharSet = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM+/=";
		
		private const int CodingBase = 36;

		public static BigInteger Decode (string input)
		{
			var reversed = input.ToLower().Reverse();
			var result = BigInteger.Zero;
			var pos = 0;
			foreach (var c in reversed)
			{
				result = BigInteger.Add(result, BigInteger.Multiply(CharSet.IndexOf(c), BigInteger.Pow(CodingBase, pos)));
				pos++;
			}
			return result;
		}
		
		public static string Encode (BigInteger input)
		{
			var result = new Stack<char>();
			while (!input.IsZero)
			{
				var index = (int)(input % CodingBase);
				result.Push(CharSet[index]);
				input = BigInteger.Divide(input, CodingBase);
			}
			return new string(result.ToArray());
		}
	}
}
