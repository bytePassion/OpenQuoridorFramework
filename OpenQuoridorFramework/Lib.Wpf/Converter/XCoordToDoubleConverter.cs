using System.Globalization;
using Lib.SemanicTypes;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class XCoordToDoubleConverter : GenericValueConverter<XCoord, double>
    {
        protected override double Convert    (XCoord value, CultureInfo culture) => value?.Value ?? 0;
		protected override XCoord ConvertBack(double value, CultureInfo culture) => new XCoord(value);
    }
}
