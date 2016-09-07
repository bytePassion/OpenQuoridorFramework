using System.Globalization;
using System.Windows;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.SingleGameVisualization.Computation
{
	internal class ComputeWallTopPosition : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? ((double)wall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3*cellHeight)
				:  (double)wall.TopLeft.YCoord      * 1.3 * cellHeight;			
		}
	}
}