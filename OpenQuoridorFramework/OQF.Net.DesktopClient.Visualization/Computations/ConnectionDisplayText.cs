using System.Globalization;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Net.DesktopClient.Contracts;
using OQF.Resources.LanguageDictionaries;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class ConnectionDisplayText : GenericTwoToOneValueConverter<ConnectionStatus, GameStatus, string>
	{
		protected override string Convert(ConnectionStatus connectionStatus, GameStatus gameStatus, CultureInfo culture)
		{
			switch (connectionStatus)
			{
				case ConnectionStatus.NotConnected:
				{
					return Captions.NCl_ConnectionStatus_NotConnected;
				}
				case ConnectionStatus.TryingToConnect:
				{
					return Captions.NCl_ConnectionStatus_TryingToConnect;
				}
				default:
				{
					return string.Empty;
				}
			}
		}
	}
}