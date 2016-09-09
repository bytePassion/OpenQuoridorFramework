using System.Globalization;
using System.Windows;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using WpfLib.ConverterBase;

namespace OQF.SingleGameVisualization.Computation
{
	internal class ComputeWallHeight : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? 0.3 * cellHeight
				: 2.3 * cellHeight;			
		}
	}
}