using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class ConnectionPanelVisibility : GenericValueConverter<ConnectionStatus, Visibility>
	{
		protected override Visibility Convert(ConnectionStatus connectionStatus, CultureInfo culture)
		{			
			return connectionStatus == ConnectionStatus.Connected 
						? Visibility.Collapsed 
						: Visibility.Visible;
		}
	}
}
