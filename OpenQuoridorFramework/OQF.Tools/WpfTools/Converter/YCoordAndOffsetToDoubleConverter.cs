using System.Globalization;
using OQF.Tools.SemanticTypes;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class YCoordAndOffsetToDoubleConverter : GenericParameterizedValueConverter<YCoord, double, double>
	{
		protected override double Convert     (YCoord value, double offset, CultureInfo culture) => value.Value + offset;
		protected override YCoord ConvertBack (double value, double offset, CultureInfo culture) => new YCoord(value - offset);
	}
}