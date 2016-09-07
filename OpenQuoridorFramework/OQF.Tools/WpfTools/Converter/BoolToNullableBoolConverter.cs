using System.Globalization;
using OQF.Tools.WpfTools.ConverterBase;

namespace OQF.Tools.WpfTools.Converter
{
	public class BoolToNullableBoolConverter : GenericValueConverter<bool, bool?>
	{
		protected override bool? Convert(bool value, CultureInfo culture)
		{
			return value;
		}

		protected override bool ConvertBack(bool? value, CultureInfo culture)
		{
			return value.HasValue && value.Value;			
		}
	}
}
