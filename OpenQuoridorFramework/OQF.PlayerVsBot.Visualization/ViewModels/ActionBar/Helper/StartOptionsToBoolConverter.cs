using System.Globalization;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.Coordination;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar.Helper
{
	internal class StartOptionsToBoolConverter : GenericValueConverter<PlayerType, bool>
    {
        protected override bool Convert(PlayerType value, CultureInfo culture)
        {
            return value == PlayerType.TopPlayer;
        }

        protected override PlayerType ConvertBack(bool value, CultureInfo culture)
        {
            return value 
				? PlayerType.TopPlayer
                : PlayerType.BottomPlayer;
        }
    }
}
