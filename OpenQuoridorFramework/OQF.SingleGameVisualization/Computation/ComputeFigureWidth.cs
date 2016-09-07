using System.Globalization;
using System.Windows;
using OQF.Contest.Contracts.GameElements;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.SingleGameVisualization.Computation
{
	internal class ComputeFigureWidth : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;

			return cellWidth - 4;
		}
	}
}