using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Visualization.Common.Global;

namespace OQF.Visualization.Common.Computation
{
	public class PlayerColor : GenericValueConverter<PlayerState, Brush>
	{
		protected override Brush Convert(PlayerState value, CultureInfo culture)
		{
			return value.Player.PlayerType == PlayerType.TopPlayer
				? Constants.Brushes.TopPlayerActiveColor
				: Constants.Brushes.BottomPlayerActiveColor;
		}
	}
}