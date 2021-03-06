﻿using System.Globalization;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ConverterBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.Computations
{
	public class ComputeWallTopPosition : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? ((double)wall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3*cellHeight)
				:  (double)wall.TopLeft.YCoord      * 1.3 * cellHeight;			
		}
	}
}