using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.WpfLib.ConverterBase;

namespace OQF.ReplayViewer.Visualization.Computations
{
	internal class BoolToBackgroundBrush : GenericValueConverter<bool, Brush>
	{
		protected override Brush Convert(bool value, CultureInfo culture)
		{
			return value 
					? new SolidColorBrush(Colors.Yellow) 
					: new SolidColorBrush(Colors.Transparent);
		}
	}
}
