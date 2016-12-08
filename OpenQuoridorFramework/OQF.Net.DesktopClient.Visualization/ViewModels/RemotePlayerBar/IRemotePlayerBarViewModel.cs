using Lib.Wpf.ViewModelBase;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar
{
	public interface IRemotePlayerBarViewModel : IViewModel
	{
		bool? IsGameInitiator { get; }
		string WallsLeft { get; }
		string PlayerName { get; }
		string PlayerStatus { get; }

		string WallsLeftLabelCaption { get; }	
	}
}