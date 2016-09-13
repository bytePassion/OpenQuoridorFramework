using System.Globalization;
using Lib.SemanicTypes;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class YCoordAndOffsetToDoubleConverter : GenericParameterizedValueConverter<YCoord, double, double>
	{
		protected override double Convert     (YCoord value, double offset, CultureInfo culture) => value.Value + offset;
		protected override YCoord ConvertBack (double value, double offset, CultureInfo culture) => new YCoord(value - offset);
	}
}