﻿using System.Globalization;
using System.Windows;
using Lib.Wpf.ConverterBase;
using OQF.Contest.Contracts.GameElements;

namespace OQF.PlayerVsBot.Computation
{
	internal class ComputeFigureHeight : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return cellHeight - 4;
		}
	}
}