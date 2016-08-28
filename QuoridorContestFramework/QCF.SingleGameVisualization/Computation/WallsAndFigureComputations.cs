using System.Globalization;
using System.Windows;
using QCF.GameEngine.Contracts.Coordination;
using QCF.GameEngine.Contracts.GameElements;
using QCF.UiTools.WpfTools.ConverterBase;

namespace QCF.SingleGameVisualization.Computation
{
	internal class ComputeFigureLeftPosition : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert(PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;
						
			return (double)currentPlayerState.Position.XCoord * 1.3 * cellWidth ;
		}
	}

	internal class ComputeFigureTopPosition : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return (double)currentPlayerState.Position.YCoord * 1.3 * cellHeight;
		}
	}

	internal class ComputeFigureHeight : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return cellHeight;
		}
	}

	internal class ComputeFigureWidth : GenericTwoToOneValueConverter<PlayerState, Size, double>
	{
		protected override double Convert (PlayerState currentPlayerState, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;

			return cellWidth;
		}
	}

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

	internal class ComputeWallTopPosition : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? ((double)wall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3*cellHeight)
				:  (double)wall.TopLeft.YCoord      * 1.3 * cellHeight;			
		}
	}

	internal class ComputeWallHeight : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellHeight = currentBoadSize.Height / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? 0.3 * cellHeight
				: 2.3 * cellHeight;			
		}
	}

	internal class ComputeWallWidth : GenericTwoToOneValueConverter<Wall, Size, double>
	{
		protected override double Convert (Wall wall, Size currentBoadSize, CultureInfo culture)
		{
			var cellWidth = currentBoadSize.Width / 11.4;

			return wall.Orientation == WallOrientation.Horizontal
				? 2.3 * cellWidth
				: 0.3 * cellWidth;
		}
	}
}
