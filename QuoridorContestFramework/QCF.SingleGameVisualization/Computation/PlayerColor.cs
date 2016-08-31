using System.Globalization;
using System.Windows.Media;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.SingleGameVisualization.Global;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
{
	internal class PlayerColor : GenericValueConverter<PlayerState, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(PlayerState value, CultureInfo culture)
		{
			return value.Player.PlayerType == PlayerType.TopPlayer
				? new SolidColorBrush(Constants.TopPlayerActiveColor)
				: new SolidColorBrush(Constants.BottomPlayerActiveColor);
		}
	}
}