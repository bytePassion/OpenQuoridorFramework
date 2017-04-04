using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.PlayerVsBot.Contracts;

namespace OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar
{
	public interface IBotStatusBarViewModel : IViewModel
	{
		GameStatus GameStatus { get; }

		
		string TopPlayerRestTime { get; }

		int TopPlayerWallCountLeft { get; }

		string BotNameLabelCaption              { get; }
		string MaximalThinkingTimeLabelCaption  { get; }
		string WallsLeftLabelCaption            { get; }
	}
}