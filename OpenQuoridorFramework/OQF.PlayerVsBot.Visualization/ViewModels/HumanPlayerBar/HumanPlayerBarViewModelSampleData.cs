using System.ComponentModel;
using System.Windows.Input;
using OQF.PlayerVsBot.Contracts;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar
{
	internal class HumanPlayerBarViewModelSampleData : IHumanPlayerBarViewModel
	{
		public HumanPlayerBarViewModelSampleData()
		{
			GameStatus = GameStatus.Active;
			BottomPlayerWallCountLeft = 9;
			MovesLeft = "97";
			WallsLeftLabelCaption = "Walls";
			CapitulateButtonCaption = "Kapitulieren";
			MovesLeftLabelCaption = "Verfügbare Züge";
		}

		public ICommand Capitulate => null;
		public GameStatus GameStatus { get; }

		public int BottomPlayerWallCountLeft { get; }
		public string MovesLeft { get; }

		public string WallsLeftLabelCaption   { get; }
		public string CapitulateButtonCaption { get; }
		public string MovesLeftLabelCaption   { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}