using System.Globalization;
using System.Windows.Media;
using OQF.SingleGameVisualization.Global;
using OQF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.SingleGameVisualization.Computation
{
	internal class TopPlayerMarkerColor : GenericValueConverter<GameStatus, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(GameStatus gameStatus, CultureInfo culture)
		{
			return gameStatus == GameStatus.Unloaded
						? Constants.PlayerInactiveColor
						: Constants.TopPlayerActiveColor;
		}
	}
}
