using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public interface IMainWindowViewModel : IViewModel
	{
		IBoardPlacementViewModel BoardPlacementViewModel { get; }
		IBoardViewModel BoardViewModel { get; }
		IProgressViewModel ProgressViewModel { get; }
		IActionBarViewModel ActionBarViewModel { get; }
		IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
		IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }
		ILocalPlayerBarViewModel LocalPlayerBarViewModel { get; }
		IRemotePlayerBarViewModel RemotePlayerBarViewModel { get; }
		INetworkViewModel NetworkViewModel { get; }
		
		bool IsProgressViewExpanded { get; }
		bool IsNetworkViewExpanded { get; }

		bool IsBoardRotated { get; }

		string ProgressCaption { get; }
		string NetworkViewCaption { get; }
	}
}