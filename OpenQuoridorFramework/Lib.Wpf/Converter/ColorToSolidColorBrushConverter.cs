using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class ColorToSolidColorBrushConverter : GenericValueConverter<Color, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(Color value, CultureInfo culture)
		{
			return new SolidColorBrush(value);
		}		
	}
}
