﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace OQF.Tools.WpfTools.ConverterBase
{
	public abstract class GenericThreeToOneValueConverter<TFrom1, TFrom2, TFrom3, TTo> : IMultiValueConverter
	{
		protected virtual TTo Convert(TFrom1 value1, TFrom2 value2, TFrom3 value3, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		protected virtual Tuple<TFrom1, TFrom2, TFrom3> ConvertBack(TTo value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 3)
				throw new ArgumentException("There sould be three values for conversion, but there are " + values.Length);

			if (values[0] != null)
				if (values[0].GetType() != typeof(TFrom1))
					if (!(values[0] is TFrom1))
						throw new ArgumentException("types are not matching: cannot convert from " + values[0].GetType() + " to " + typeof(TFrom1));

			if (values[1] != null)
				if (values[1].GetType() != typeof(TFrom2))
					if (!(values[1] is TFrom2))
						throw new ArgumentException("types are not matching: cannot convert from " + values[1].GetType() + " to " + typeof(TFrom2));

			if (values[2] != null)
				if (values[2].GetType() != typeof(TFrom3))
					if (!(values[2] is TFrom3))
						throw new ArgumentException("types are not matching: cannot convert from " + values[2].GetType() + " to " + typeof(TFrom3));

			return Convert((TFrom1)values[0], (TFrom2)values[1], (TFrom3)values[2], culture);			
		}

		public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			if (value != null)
				if (value.GetType() != typeof(TTo))
					if (!(value is TTo))
						throw new ArgumentException("types are not matching: cannot convert from " + value.GetType() + " to " + typeof(TTo));

			var conversionResult = ConvertBack((TTo)value, culture);

			return new object[]
			       {
				       conversionResult.Item1, 
					   conversionResult.Item2, 
					   conversionResult.Item3
			       };
		}
	}
}