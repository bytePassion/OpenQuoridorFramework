using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.PlayerVsBot.Global;

namespace OQF.PlayerVsBot.Computation
{
	//	internal class ComputeWallWidth : GenericTwoToOneValueConverter<Wall, Size, double>
	//	{
	//		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
	//		{
	//			var cellWidth = currentBoadSize.Width / 11.4;
	//
	//			return wall.Orientation == WallOrientation.Horizontal
	//				? 2.3 * cellWidth
	//				: 0.3 * cellWidth;
	//		}
	//	}

	internal class ComputeWallWidth : IMultiValueConverter
	{
		public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var wall = (Wall) values[0];
			var currentBoardSize = values[1] as Size? ?? Constants.SizeFallBackValue;

			var cellWidth = currentBoardSize.Width / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? 2.3 * cellWidth
				: 0.3 * cellWidth;
		}

		public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
