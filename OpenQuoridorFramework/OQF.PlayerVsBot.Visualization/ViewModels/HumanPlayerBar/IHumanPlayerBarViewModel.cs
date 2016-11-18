using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.PlayerVsBot.Contracts;

namespace OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar
{
	public interface IHumanPlayerBarViewModel : IViewModel
	{
		ICommand Capitulate { get; }

		GameStatus GameStatus { get; }

		int BottomPlayerWallCountLeft { get; }
		string MovesLeft { get; }

		string WallsLeftLabelCaption   { get; }
		string CapitulateButtonCaption { get; }
		string MovesLeftLabelCaption   { get; }
	}
}