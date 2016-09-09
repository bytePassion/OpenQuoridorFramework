using System.Globalization;
using SemanicTypesLib.Base;
using WpfLib.ConverterBase;

namespace WpfLib.Converter
{
	public class SemanticDoubleTypeToDoubleConverter : GenericValueConverter<SemanticType<double>, double>
    {
        protected override double Convert(SemanticType<double> value, CultureInfo culture)
        {
            return value.Value;
        }
    }
}
