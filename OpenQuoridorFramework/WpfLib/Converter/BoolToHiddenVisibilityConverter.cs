using System.Globalization;
using System.Windows;
using WpfLib.ConverterBase;

namespace WpfLib.Converter
{
	public class BoolToHiddenVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
		    return value ? Visibility.Visible : Visibility.Hidden;
	    }	   
    }
}
