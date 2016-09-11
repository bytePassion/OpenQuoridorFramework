using System.Collections.Generic;
using System.Linq;

namespace OQF.Utils
{
	public static class ParseProcessText
	{
		public static IEnumerable<string> FromFileText(string fileText)
		{
			var result = new List<string>();

			var lines = fileText.Split('\n');

			foreach (var line in lines)
			{
				var lineParts = line.Split(' ')
									.Select(element => element.Trim())
									.ToList();

				if (lineParts.Count == 3)
				{
					if (!string.IsNullOrWhiteSpace(lineParts[1])) result.Add(lineParts[1]);
					if (!string.IsNullOrWhiteSpace(lineParts[2])) result.Add(lineParts[2]);
				}

				if (lineParts.Count == 2)
				{
					if (!string.IsNullOrWhiteSpace(lineParts[1])) result.Add(lineParts[1]);					
				}
			}

			return result;
		} 
	}
}
