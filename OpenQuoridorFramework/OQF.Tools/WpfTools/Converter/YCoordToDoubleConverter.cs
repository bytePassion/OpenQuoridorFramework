using System.Globalization;
using OQF.Tools.SemanticTypes;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class YCoordToDoubleConverter : GenericValueConverter<YCoord, double>
    {
        protected override double Convert    (YCoord value, CultureInfo culture) => value.Value;
	    protected override YCoord ConvertBack(double value, CultureInfo culture) => new YCoord(value);
    }
}