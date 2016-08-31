using System.Globalization;
using QCF.Tools.SemanticTypes;
using QCF.Tools.WpfTools.ConverterBase;

namespace QCF.Tools.WpfTools.Converter
{
	public class XCoordToDoubleConverter : GenericValueConverter<XCoord, double>
    {
        protected override double Convert    (XCoord value, CultureInfo culture) => value?.Value ?? 0;
		protected override XCoord ConvertBack(double value, CultureInfo culture) => new XCoord(value);
    }
}
