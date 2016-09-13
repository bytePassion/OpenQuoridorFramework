using System;
using System.Globalization;
using System.Windows.Data;

namespace Lib.Wpf.ConverterBase
{
	public abstract class GenericParameterizedValueConverter <TFrom, TTo, TParam> : IValueConverter
    {
	    protected virtual TTo Convert(TFrom value, TParam parameter, CultureInfo culture)
	    {
			throw new NotImplementedException();
	    }

	    protected virtual TFrom ConvertBack(TTo value, TParam parameter, CultureInfo culture)
	    {
			throw new NotImplementedException();
	    }

	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			if (parameter != null)
				if (parameter.GetType() != typeof(TParam))
					if (!(parameter is TParam))
						throw new ArgumentException("types are not matching: cannot convert from " + parameter.GetType() + " to " + typeof(TParam));

			if (value != null)
				if (value.GetType() != typeof(TFrom))
					if (!(value is TFrom))
						throw new ArgumentException("types are not matching: cannot convert from " + value.GetType() + " to " + typeof(TFrom));

			return Convert((TFrom)value,(TParam) parameter, culture);			
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			if (value != null)
				if (value.GetType() != typeof(TTo))
					if (!(value is TTo))
						throw new ArgumentException("types are not matching: cannot convert from " + value.GetType() + " to " + typeof(TTo));

			if (parameter != null)
				if (parameter.GetType() != typeof(TParam))
					if (!(parameter is TParam))
						throw new ArgumentException("types are not matching: cannot convert from " + parameter.GetType() + " to " + typeof(TParam));

			return ConvertBack((TTo)value, (TParam)parameter, culture);			
	    }
    }
}
