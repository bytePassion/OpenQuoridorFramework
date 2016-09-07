using System.Globalization;
using System.Windows.Media;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class ColorToSolidColorBrushConverter : GenericValueConverter<Color, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(Color value, CultureInfo culture)
		{
			return new SolidColorBrush(value);
		}		
	}
}
