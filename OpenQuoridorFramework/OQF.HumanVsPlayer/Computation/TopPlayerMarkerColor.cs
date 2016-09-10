﻿using System.Globalization;
using System.Windows.Media;
using OQF.HumanVsPlayer.Global;
using OQF.HumanVsPlayer.ViewModels.MainWindow.Helper;
using WpfLib.ConverterBase;

namespace OQF.HumanVsPlayer.Computation
{
	internal class TopPlayerMarkerColor : GenericValueConverter<GameStatus, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(GameStatus gameStatus, CultureInfo culture)
		{
			return gameStatus == GameStatus.Unloaded
						? Constants.PlayerInactiveColor
						: Constants.TopPlayerActiveColor;
		}
	}
}