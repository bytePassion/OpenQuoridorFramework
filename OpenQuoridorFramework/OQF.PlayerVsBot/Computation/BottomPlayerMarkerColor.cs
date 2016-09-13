using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.PlayerVsBot.Global;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;

namespace OQF.PlayerVsBot.Computation
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