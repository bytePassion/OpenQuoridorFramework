using System.Globalization;
using Lib.Wpf.ConverterBase;

namespace OQF.PlayerVsBot.Visualization.Computations
{
	internal class GetTopPlayerMoveFromString : GenericValueConverter<string, string>
	{
		protected override string Convert (string value, CultureInfo culture)
		{
			var parts = value.Trim().Split(' ');

			return parts.Length > 2
				? parts[2]
				: string.Empty;
		}
	}
}