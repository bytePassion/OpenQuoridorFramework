using System.Collections.Generic;
using System.Linq;

namespace OQF.Utils
{
	public static class ParseProgressText
	{
		public static IEnumerable<string> FromFileText(string fileText)
		{
			var result = new List<string>();

			var lines = fileText.Split('\n')
								.Select(element => element.Trim());

			var currentLineNumber = 0;

			foreach (var line in lines)
			{
				var lineParts = line.Split(' ')
									.Select(element => element.Trim())
									.ToList();

				if (!(lineParts.Count == 3 || lineParts.Count == 2))
					continue;

				var nextLineNumberAsString = lineParts[0].Substring(0, lineParts[0].Length - 1);
				int nextLineNumber;

				if (!int.TryParse(nextLineNumberAsString, out nextLineNumber))
				{
					return new List<string>();
				}

				if (nextLineNumber != ++currentLineNumber)
				{
					return new List<string>();
				}

				if (lineParts.Count == 3)
				{
					if (MoveValidator.IsValidMove(lineParts[1]) &&
					    MoveValidator.IsValidMove(lineParts[2]))
					{
						result.Add(lineParts[1]);
						result.Add(lineParts[2]);						
					}
					else
					{
						return new List<string>();
					}
										
				}

				if (lineParts.Count == 2)
				{
					if (MoveValidator.IsValidMove(lineParts[1]))
					{
						result.Add(lineParts[1]);
					}
					else
					{
						return new List<string>();
					}					
				}
			}

			return result;
		}		
	}
}
