using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.PlayerVsBot.ViewModels.MainWindow.Helper;

namespace OQF.PlayerVsBot.Computations
{
	internal class TopPlayerMarkerColor : GenericValueConverter<GameStatus, Brush>
	{
		protected override Brush Convert(GameStatus gameStatus, CultureInfo culture)
		{
			return gameStatus == GameStatus.Unloaded
						? Visualization.Common.Board.BoardColors.Brushes.PlayerInactiveColor
						: Visualization.Common.Board.BoardColors.Brushes.TopPlayerActiveColor;			
		}
	}
}
