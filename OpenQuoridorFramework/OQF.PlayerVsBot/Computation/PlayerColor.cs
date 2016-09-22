using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.PlayerVsBot.Global;

namespace OQF.PlayerVsBot.Computation
{
	internal class PlayerColor : GenericValueConverter<PlayerState, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(PlayerState value, CultureInfo culture)
		{
			return value.Player.PlayerType == PlayerType.TopPlayer
				? Constants.TopPlayerActiveColor
				: Constants.BottomPlayerActiveColor;
		}
	}
}