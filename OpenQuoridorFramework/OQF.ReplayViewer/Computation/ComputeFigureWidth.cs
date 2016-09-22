using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.ReplayViewer.Computation
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