using System.Globalization;
using OQF.Tools.SemanticTypes;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class XCoordToDoubleConverter : GenericValueConverter<XCoord, double>
    {
        protected override double Convert    (XCoord value, CultureInfo culture) => value?.Value ?? 0;
		protected override XCoord ConvertBack(double value, CultureInfo culture) => new XCoord(value);
    }
}
