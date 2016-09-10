﻿using System.Globalization;
using System.Windows;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using WpfLib.ConverterBase;

namespace OQF.HumanVsPlayer.Computation
{
	internal class ComputeWallLeftPosition : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				?  (double) wall.TopLeft.XCoord      * 1.3 * cellWidth
				: ((double) wall.TopLeft.XCoord + 1) * 1.3 * cellWidth - (0.3*cellWidth);
		}
	}
}