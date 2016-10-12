using System.Globalization;
using Lib.Wpf.ConverterBase;

namespace OQF.PlayerVsBot.Visualization.Computations
{
	internal class GetBottomPlayerMoveFromString : GenericValueConverter<string, string>
	{
		protected override string Convert (string value, CultureInfo culture)
		{
			var parts = value.Trim().Split(' ');

			return parts.Length > 1
				? parts[1]
				: string.Empty;
		}
	}
}