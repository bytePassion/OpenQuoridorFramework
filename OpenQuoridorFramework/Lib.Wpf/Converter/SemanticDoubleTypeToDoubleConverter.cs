using System.Globalization;
using Lib.SemanicTypes.Base;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class SemanticDoubleTypeToDoubleConverter : GenericValueConverter<SemanticType<double>, double>
    {
        protected override double Convert(SemanticType<double> value, CultureInfo culture)
        {
            return value.Value;
        }
    }
}
