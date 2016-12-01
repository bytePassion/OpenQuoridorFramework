using System;
using System.Globalization;
using System.Windows.Media;
using Lib.Wpf.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class ConnectionDisplayBackground : GenericValueConverter<ConnectionStatus, Brush>
	{
		protected override Brush Convert(ConnectionStatus value, CultureInfo culture)
		{
			switch (value)
			{
				case ConnectionStatus.Connected:       return new SolidColorBrush(Colors.LawnGreen);
				case ConnectionStatus.NotConnected:    return new SolidColorBrush(Colors.Red);
				case ConnectionStatus.TryingToConnect: return new SolidColorBrush(Colors.Orange);
			}

			throw new ArgumentException();
		}
	}
}