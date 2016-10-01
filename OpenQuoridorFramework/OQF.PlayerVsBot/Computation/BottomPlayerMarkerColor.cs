using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;

namespace OQF.PlayerVsBot.Computation
{
	internal class BottomPlayerMarkerColor : GenericValueConverter<GameStatus, Brush>
	{
		protected override Brush Convert (GameStatus gameStatus, CultureInfo culture)
		{
//			return gameStatus == GameStatus.Unloaded
//						?  BoardColors.Brushes.PlayerInactiveColor
//						: Constants.Brushes.BottomPlayerActiveColor;

			return null;
		}
	}
}