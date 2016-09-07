using System.Globalization;
using System.Windows;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class InvertedBoolToCollapsedVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
			return value ? Visibility.Collapsed : Visibility.Visible;
	    }	    
    }
}
