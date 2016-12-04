using System;
using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class CancelGamePanelVisibility : GenericTwoToOneValueConverter<ConnectionStatus, GameStatus, Visibility>
	{
		protected override Visibility Convert (ConnectionStatus connectionStatus, GameStatus gameStatus, CultureInfo culture)
		{
			if (connectionStatus != ConnectionStatus.Connected)
				return Visibility.Collapsed;

			switch (gameStatus)
			{
			
				case GameStatus.WaitingForOponend: return Visibility.Visible;

				case GameStatus.NoGame:
				case GameStatus.GameOver:
				case GameStatus.PlayingJoinedGame:
				case GameStatus.PlayingOpendGame:  return Visibility.Collapsed;
			}

			throw new ArgumentException();
		}
	}
}