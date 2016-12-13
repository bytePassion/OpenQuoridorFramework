using System.Globalization;
using Lib.Wpf.ConverterBase;
using OQF.Bot.Contracts.Coordination;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar.Helper
{
    class StartOptionsToBoolConverter : GenericValueConverter<StartOptionsDisplayData, bool>
    {
        protected override bool Convert(StartOptionsDisplayData value, CultureInfo culture)
        {
            return value.PlayerStartingType != PlayerType.TopPlayer;
        }

        protected override StartOptionsDisplayData ConvertBack(bool value, CultureInfo culture)
        {
            return value ? new StartOptionsDisplayData(PlayerType.BottomPlayer,string.Empty) 
                : new StartOptionsDisplayData(PlayerType.TopPlayer, string.Empty);
        }
    }
}
