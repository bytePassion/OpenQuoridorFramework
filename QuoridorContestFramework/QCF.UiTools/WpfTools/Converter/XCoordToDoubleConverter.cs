using System.Globalization;
using QCF.UiTools.SemanticTypes;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.UiTools.WpfTools.Converter
{
	public class XCoordToDoubleConverter : GenericValueConverter<XCoord, double>
    {
        protected override double Convert    (XCoord value, CultureInfo culture) => value?.Value ?? 0;
		protected override XCoord ConvertBack(double value, CultureInfo culture) => new XCoord(value);
    }
}
