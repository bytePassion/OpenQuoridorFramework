using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.Computations
{
	public class PlayerColor : GenericValueConverter<PlayerState, Brush>
	{
		protected override Brush Convert(PlayerState value, CultureInfo culture)
		{
			return value.Player.PlayerType == PlayerType.TopPlayer
				? BoardColors.Brushes.TopPlayerActiveColor
				: BoardColors.Brushes.BottomPlayerActiveColor;		
		}
	}
}