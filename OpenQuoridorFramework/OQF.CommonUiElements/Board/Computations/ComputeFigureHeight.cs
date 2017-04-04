using System.Globalization;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.Computations
{
	public class ComputeFigureHeight : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return cellHeight - 4;
		}
	}
}