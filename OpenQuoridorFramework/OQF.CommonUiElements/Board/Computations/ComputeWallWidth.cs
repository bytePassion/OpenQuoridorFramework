using System.Globalization;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.Computations
{
	public class ComputeWallWidth : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? 2.3 * cellWidth
				: 0.3 * cellWidth;
		}
	}
}
