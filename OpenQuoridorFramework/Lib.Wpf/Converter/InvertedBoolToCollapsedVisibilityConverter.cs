using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class InvertedBoolToCollapsedVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
			return value ? Visibility.Collapsed : Visibility.Visible;
	    }	    
    }
}
