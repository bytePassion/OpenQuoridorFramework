using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfLib.ConverterBase;

namespace bytePassion.OnkoTePla.Resources.MainWindow.Converter
{
    internal class MinimizeVisibilityConverter : GenericValueConverter<ResizeMode, Visibility>
    {
        protected override Visibility Convert(ResizeMode value, CultureInfo culture)
        {
            return value == ResizeMode.NoResize 
                                ? Visibility.Collapsed 
                                : Visibility.Visible;
        }       
    }
}
