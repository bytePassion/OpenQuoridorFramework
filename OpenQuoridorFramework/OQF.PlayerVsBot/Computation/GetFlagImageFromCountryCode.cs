using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lib.Wpf.ConverterBase;

namespace OQF.PlayerVsBot.Computation
{
	internal class GetFlagImageFromCountryCode : GenericValueConverter<string, ImageSource>
	{
		protected override ImageSource Convert(string value, CultureInfo culture)
		{
			ImageSource flagIcon;
			try
			{
				flagIcon = new BitmapImage(new Uri($"pack://application:,,,/OQF.Visualization.Resources;Component/FlagIcons/{value}.png"));
			}
			catch 
			{				
				flagIcon = new BitmapImage();
			}

			return flagIcon;
		}
	}
}
