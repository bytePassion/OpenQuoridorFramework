using System;
using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Visualization.Computations
{
	internal class CreateGamePanelVisibility : GenericTwoToOneValueConverter<ConnectionStatus, GameStatus, Visibility>
	{
		protected override Visibility Convert(ConnectionStatus connectionStatus, GameStatus gameStatus, CultureInfo culture)
		{
			if (connectionStatus != ConnectionStatus.Connected)
				return Visibility.Collapsed;

			switch (gameStatus)
			{
				case GameStatus.GameOver:				
				case GameStatus.NoGame:             return Visibility.Visible;

				case GameStatus.WaitingForOponend:
				case GameStatus.PlayingJoinedGame:
				case GameStatus.PlayingOpendGame:   return Visibility.Collapsed;
			}

			throw new ArgumentException();
		}
	}
}