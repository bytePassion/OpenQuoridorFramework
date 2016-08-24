﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace QCF.UiTools.WpfTools.ConverterBase
{
	public abstract class GenericTwoToOneValueConverter <TFrom1, TFrom2, TTo> : IMultiValueConverter
    {
	    protected virtual TTo Convert(TFrom1 value1, TFrom2 value2, CultureInfo culture)
	    {
			throw new NotImplementedException();
	    }

	    protected virtual Tuple<TFrom1, TFrom2> ConvertBack(TTo value, CultureInfo culture)
	    {
			throw new NotImplementedException();
	    }   	   

	    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	    {
			if (values.Length != 2)
				throw new ArgumentException("There sould be two values for conversion, but there are " + values.Length);

			if (values[0] != null)
				if (values[0].GetType() != typeof(TFrom1))
					if (!(values[0] is TFrom1))
						throw new ArgumentException("types are not matching: cannot convert from " + values[0].GetType() + " to " + typeof(TFrom1));

			if (values[1] != null)
				if (values[1].GetType() != typeof(TFrom2))
					if (!(values[1] is TFrom2))
						throw new ArgumentException("types are not matching: cannot convert from " + values[1].GetType() + " to " + typeof(TFrom2));

			return Convert((TFrom1)values[0], (TFrom2)values[1], culture);			
	    }

	    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	    {
			if (value.GetType() != typeof(TTo))
				if (!(value is TTo))
					throw new ArgumentException("types are not matching");


			var conversionResult = ConvertBack((TTo)value, culture);

			return new object[]
			       {
				       conversionResult.Item1, 
					   conversionResult.Item2
			       };
	    }
    }
}
