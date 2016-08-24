using System.Globalization;
using QCF.UiTools.SemanticTypes;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.UiTools.WpfTools.Converter
{
	public class YCoordToDoubleConverter : GenericValueConverter<YCoord, double>
    {
        protected override double Convert    (YCoord value, CultureInfo culture) => value.Value;
	    protected override YCoord ConvertBack(double value, CultureInfo culture) => new YCoord(value);
    }
}