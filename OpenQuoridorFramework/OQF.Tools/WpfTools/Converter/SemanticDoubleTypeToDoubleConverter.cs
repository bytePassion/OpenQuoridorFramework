using System.Globalization;
using OQF.Tools.SemanticTypes.Base;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class SemanticDoubleTypeToDoubleConverter : GenericValueConverter<SemanticType<double>, double>
    {
        protected override double Convert(SemanticType<double> value, CultureInfo culture)
        {
            return value.Value;
        }
    }
}
