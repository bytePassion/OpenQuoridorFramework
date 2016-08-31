using System.Globalization;
using System.Windows;
using QCF.Contest.Contracts.GameElements;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
{
	internal class ComputeFigureHeight : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return cellHeight - 4;
		}
	}
}