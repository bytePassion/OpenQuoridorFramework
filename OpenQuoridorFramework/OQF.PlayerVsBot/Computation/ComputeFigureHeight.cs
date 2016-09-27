using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using OQF.Bot.Contracts.GameElements;
using OQF.PlayerVsBot.Global;

namespace OQF.PlayerVsBot.Computation
{
	//	internal class ComputeFigureHeight : GenericTwoToOneValueConverter<PlayerState, Size, double>
	//	{
	//		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
	//		{
	//			var cellHeight = currentBoadSize.Height / 11.4;
	//
	//			return cellHeight - 4;
	//		}
	//	}

	internal class ComputeFigureHeight : IMultiValueConverter
	{		
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{			
			var currentPlayerState = (PlayerState) values[0];			
			var currentBoardSize = values[1] as Size? ?? Constants.FallBacks.SizeFallBackValue;

			var cellHeight = currentBoardSize.Height / 11.4;

			return cellHeight - 4;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}