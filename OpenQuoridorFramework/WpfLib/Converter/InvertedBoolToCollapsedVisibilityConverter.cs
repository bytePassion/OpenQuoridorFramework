using System.Globalization;
using System.Windows;
using WpfLib.ConverterBase;

namespace WpfLib.Converter
{
	public class InvertedBoolToCollapsedVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
			return value ? Visibility.Collapsed : Visibility.Visible;
	    }	    
    }
}
