using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.PlayerVsBot.Visualization.ViewModels.MainWindow.Helper;

namespace OQF.PlayerVsBot.Visualization.Computations
{
	internal class BottomPlayerMarkerColor : GenericValueConverter<GameStatus, Brush>
	{
		protected override Brush Convert (GameStatus gameStatus, CultureInfo culture)
		{
			return gameStatus == GameStatus.Unloaded
						? CommonUiElements.Board.BoardColors.Brushes.PlayerInactiveColor
						: CommonUiElements.Board.BoardColors.Brushes.BottomPlayerActiveColor;			
		}
	}
}