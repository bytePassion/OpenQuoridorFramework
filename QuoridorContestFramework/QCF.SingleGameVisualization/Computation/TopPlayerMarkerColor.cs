using System.Globalization;
using System.Windows.Media;
using QCF.SingleGameVisualization.Global;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
{
	internal class TopPlayerMarkerColor : GenericValueConverter<bool, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(bool isGameRunning, CultureInfo culture)
		{
			return isGameRunning
						? new SolidColorBrush(Constants.TopPlayerActiveColor)
						: new SolidColorBrush(Constants.PlayerInactiveColor);
		}
	}
}
