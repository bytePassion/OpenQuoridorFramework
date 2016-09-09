using System.Globalization;
using System.Windows;
using OQF.Contest.Contracts.GameElements;
using WpfLib.ConverterBase;

namespace OQF.SingleGameVisualization.Computation
{
	internal class ComputeFigureLeftPosition : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert(PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;
						
			return (double)currentPlayerState.Position.XCoord * 1.3 * cellWidth + 1;
		}
	}
}