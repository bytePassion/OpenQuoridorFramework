using System.Globalization;
using Lib.Wpf.ConverterBase;

namespace OQF.PlayerVsBot.Visualization.Computations
{
	internal class GetProgressNrFromString : GenericValueConverter<string, string>
	{
		protected override string Convert(string value, CultureInfo culture)
		{
			var parts = value.Trim().Split(' ');

			return parts.Length > 0 
				? parts[0] 
				: string.Empty;
		}
	}
}
