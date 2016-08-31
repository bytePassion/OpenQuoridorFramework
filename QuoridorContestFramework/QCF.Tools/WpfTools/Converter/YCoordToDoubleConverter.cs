using System.Globalization;
using QCF.Tools.SemanticTypes;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.Tools.WpfTools.Converter
{
	public class YCoordToDoubleConverter : GenericValueConverter<YCoord, double>
    {
        protected override double Convert    (YCoord value, CultureInfo culture) => value.Value;
	    protected override YCoord ConvertBack(double value, CultureInfo culture) => new YCoord(value);
    }
}