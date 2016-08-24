using System.Globalization;
using QCF.UiTools.SemanticTypes.Base;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.UiTools.WpfTools.Converter
{
	public class SemanticDoubleTypeToDoubleConverter : GenericValueConverter<SemanticType<double>, double>
    {
        protected override double Convert(SemanticType<double> value, CultureInfo culture)
        {
            return value.Value;
        }
    }
}
