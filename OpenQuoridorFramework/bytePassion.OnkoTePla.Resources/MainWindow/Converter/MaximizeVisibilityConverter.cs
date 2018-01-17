using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfLib.ConverterBase;

namespace bytePassion.OnkoTePla.Resources.MainWindow.Converter
{
    internal class MaximizeVisibilityConverter : GenericTwoToOneValueConverter<ResizeMode, WindowState, Visibility>
    {
        protected override Visibility Convert(ResizeMode resizeMode, WindowState windowState, CultureInfo culture)
        {
            if (resizeMode != ResizeMode.NoResize &&
                resizeMode != ResizeMode.CanMinimize &&
                windowState != WindowState.Maximized)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }       
    }
}