using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.ReplayViewer.Global;

namespace OQF.ReplayViewer.Computation
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