using System.Globalization;
using System.Windows.Media;
using QCF.SingleGameVisualization.Global;
using QCF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
{
	internal class BottomPlayerMarkerColor : GenericValueConverter<GameStatus, SolidColorBrush>
	{
		protected override SolidColorBrush Convert (GameStatus gameStatus, CultureInfo culture)
		{
			return gameStatus == GameStatus.Unloaded
						? Constants.PlayerInactiveColor
						: Constants.BottomPlayerActiveColor;
		}
	}
}