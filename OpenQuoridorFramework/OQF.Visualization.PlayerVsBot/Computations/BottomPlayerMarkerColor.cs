using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Visualization.PlayerVsBot.ViewModels.MainWindow.Helper;

namespace OQF.Visualization.PlayerVsBot.Computations
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