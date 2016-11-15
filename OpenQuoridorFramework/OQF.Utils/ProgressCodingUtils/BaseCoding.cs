#region License
/* The MIT License (MIT)
 * Copyright (c) 2011 Michael Stum, http://www.Stum.de <opensource@stum.de>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OQF.Utils.ProgressCodingUtils
{
	public static class BaseCoding
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
				result = BigInteger.Add(result, 
										BigInteger.Multiply(CharSet.IndexOf(c), 
															BigInteger.Pow(CodingBase, pos)));
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
