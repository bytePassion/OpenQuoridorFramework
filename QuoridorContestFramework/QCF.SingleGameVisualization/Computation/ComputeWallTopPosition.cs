using System.Globalization;
using System.Windows;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
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