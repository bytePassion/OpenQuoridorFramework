using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.WpfLib.ConverterBase;
using static OQF.CommonUiElements.Board.BoardColors.Brushes;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class GetPlayerIconBrush : GenericValueConverter<bool?, Brush>
	{
		protected override Brush Convert(bool? isPlayerGameInitiator, CultureInfo culture)
		{
			if (isPlayerGameInitiator.HasValue)
			{
				return isPlayerGameInitiator.Value 
							? BottomPlayerActiveColor 
							: TopPlayerActiveColor;				
			}
			else
			{
				return PlayerInactiveColor;
			}
		}
	}
}
