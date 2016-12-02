using System;
using System.Globalization;
using Lib.Wpf.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class ConnectionDisplayText : GenericTwoToOneValueConverter<ConnectionStatus, GameStatus, string>
	{
		protected override string Convert(ConnectionStatus connectionStatus, GameStatus gameStatus, CultureInfo culture)
		{
			switch (connectionStatus)
			{
				case ConnectionStatus.Connected:
				{
					if (gameStatus == GameStatus.WaitingForOponend)
						return "Connected - Waiting for opponend";
					else
						return "Connected";
				}
				case ConnectionStatus.NotConnected:
				{
					return "Not Connected";
				}
				case ConnectionStatus.TryingToConnect:
				{
					return "Trying to connect ....";
				}
			}

			throw new ArgumentException();
		}
	}
}