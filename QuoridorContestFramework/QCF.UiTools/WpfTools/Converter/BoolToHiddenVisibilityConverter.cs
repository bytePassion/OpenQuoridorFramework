using System.Globalization;
using System.Windows;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.UiTools.WpfTools.Converter
{
	public class BoolToHiddenVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
		    return value ? Visibility.Visible : Visibility.Hidden;
	    }	   
    }
}
