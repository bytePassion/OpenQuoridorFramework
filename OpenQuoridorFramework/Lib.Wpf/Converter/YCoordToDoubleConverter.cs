using System.Globalization;
using Lib.SemanicTypes;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class YCoordToDoubleConverter : GenericValueConverter<YCoord, double>
    {
        protected override double Convert    (YCoord value, CultureInfo culture) => value.Value;
	    protected override YCoord ConvertBack(double value, CultureInfo culture) => new YCoord(value);
    }
}