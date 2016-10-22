using System.Globalization;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.Computations
{
    public class ComputeFigureTopPosition : GenericTwoToOneValueConverter<PlayerState, Lib.SemanicTypes.Size, double>
    {
        protected override double Convert(PlayerState currentPlayerState, Lib.SemanicTypes.Size currentBoadSize, CultureInfo culture)
        {
            var cellHeight = currentBoadSize.Height / 11.4;

            return (double)currentPlayerState.Position.YCoord * 1.3 * cellHeight + 1;
        }
    }
}