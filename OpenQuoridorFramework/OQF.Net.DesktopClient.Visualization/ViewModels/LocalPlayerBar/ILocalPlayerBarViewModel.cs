using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	public interface ILocalPlayerBarViewModel : IViewModel
	{
		ICommand Capitulate { get; }
		
		bool? IsGameInitiator { get; }
		string WallsLeft { get; }
		string PlayerName { get; }
		string PlayerStatus { get; }

		string WallsLeftLabelCaption { get; }
		string CapitulateButtonCaption { get; }
	}
}