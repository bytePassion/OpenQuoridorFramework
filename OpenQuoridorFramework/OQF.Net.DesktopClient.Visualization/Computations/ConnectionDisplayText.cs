using System;
using System.Globalization;
using Lib.Wpf.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class ConnectionDisplayText : GenericValueConverter<ConnectionStatus, string>
	{
		protected override string Convert(ConnectionStatus value, CultureInfo culture)
		{
			switch (value)
			{
				case ConnectionStatus.Connected:       { return "Connected"; }
				case ConnectionStatus.NotConnected:    { return "Notconnected"; }
				case ConnectionStatus.TryingToConnect: { return "Trying to connect ...."; }
			}

			throw new ArgumentException();
		}
	}
}