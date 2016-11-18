using System.ComponentModel;
using OQF.PlayerVsBot.Contracts;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar
{
	internal class BotStatusBarViewModelSampleData : IBotStatusBarViewModel
	{
		public BotStatusBarViewModelSampleData()
		{
			GameStatus = GameStatus.Active;
			
			TopPlayerRestTime = "36";
			TopPlayerWallCountLeft = 10;

			BotNameLabelCaption             = "BotName";
			MaximalThinkingTimeLabelCaption = "max. Rechenzeit";
			WallsLeftLabelCaption           = "Walls";
		}

		public GameStatus GameStatus { get; }

		
		public string TopPlayerRestTime      { get; }
		public int    TopPlayerWallCountLeft { get; }

		public string BotNameLabelCaption             { get; }
		public string MaximalThinkingTimeLabelCaption { get; }
		public string WallsLeftLabelCaption           { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}