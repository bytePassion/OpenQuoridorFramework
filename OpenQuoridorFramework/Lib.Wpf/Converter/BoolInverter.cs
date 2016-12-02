﻿using System.Globalization;
using Lib.Wpf.ConverterBase;

namespace Lib.Wpf.Converter
{
	public class BoolInverter : GenericValueConverter<bool, bool>
	{
		protected override bool Convert(bool value, CultureInfo culture)
		{
			return !value;
		}

		protected override bool ConvertBack(bool value, CultureInfo culture)
		{
			return !value;
		}
	}
}
