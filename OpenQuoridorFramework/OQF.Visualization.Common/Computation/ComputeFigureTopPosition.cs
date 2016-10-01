using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.Visualization.Common.Computation
{
	public class ComputeFigureTopPosition : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return (double)currentPlayerState.Position.YCoord * 1.3 * cellHeight + 1;
		}
	}
}