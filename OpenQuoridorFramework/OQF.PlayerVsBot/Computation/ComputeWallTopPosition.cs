using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.PlayerVsBot.Global;

namespace OQF.PlayerVsBot.Computation
{
	//	internal class ComputeWallTopPosition : GenericTwoToOneValueConverter<Wall, Size, double>
	//	{
	//		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
	//		{
	//			var cellHeight = currentBoadSize.Height / 11.4;
	//
	//			return wall.Orientation == WallOrientation.Horizontal
	//				? ((double)wall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3*cellHeight)
	//				:  (double)wall.TopLeft.YCoord      * 1.3 * cellHeight;			
	//		}
	//	}

	internal class ComputeWallTopPosition : IMultiValueConverter
	{
		public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var wall = (Wall) values[0];
			var currentBoardSize = values[1] as Size? ?? Constants.FallBacks.SizeFallBackValue;

			var cellHeight = currentBoardSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? ((double)wall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3 * cellHeight)
				: (double)wall.TopLeft.YCoord * 1.3 * cellHeight;
		}

		public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}