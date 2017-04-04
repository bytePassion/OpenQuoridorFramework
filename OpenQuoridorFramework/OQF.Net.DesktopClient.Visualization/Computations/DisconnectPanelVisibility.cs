﻿using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class DisconnectPanelVisibility : GenericValueConverter<ConnectionStatus, Visibility>
	{
		protected override Visibility Convert(ConnectionStatus connectionStatus, CultureInfo culture)
		{
			return connectionStatus == ConnectionStatus.Connected 
				? Visibility.Visible 
				: Visibility.Collapsed;
		}
	}
}