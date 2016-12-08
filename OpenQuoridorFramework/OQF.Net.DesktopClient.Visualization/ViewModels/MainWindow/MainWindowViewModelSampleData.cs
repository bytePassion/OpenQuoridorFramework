using System.ComponentModel;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			BoardPlacementViewModel = new BoardPlacementViewModelSampleData();
			BoardViewModel = new BoardViewModelSampleData();
			ProgressViewModel = new ProgressViewModelSampleData();
			ActionBarViewModel = new ActionBarViewModelSampleData();
			BoardHorizontalLabelingViewModel = new BoardLabelingViewModelSampleData();
			BoardVerticalLabelingViewModel = new BoardLabelingViewModelSampleData();
			LocalPlayerBarViewModel = new LocalPlayerBarViewModelSampleData();
			RemotePlayerBarViewModel = new RemotePlayerBarViewModelSampleData();
			NetworkViewModel = new NetworkViewModelSampleData();
			
			IsBoardRotated = false;

			IsProgressViewExpanded = false;
			IsNetworkViewExpanded = true;

			ProgressCaption = "Spielfortschritt";
			NetworkViewCaption = "Verbindungsoptionen";
		}

		public IBoardPlacementViewModel BoardPlacementViewModel { get; }
		public IBoardViewModel BoardViewModel { get; }
		public IProgressViewModel ProgressViewModel { get; }
		public IActionBarViewModel ActionBarViewModel { get; }
		public IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }
		public ILocalPlayerBarViewModel LocalPlayerBarViewModel { get; }
		public IRemotePlayerBarViewModel RemotePlayerBarViewModel { get; }
		public INetworkViewModel NetworkViewModel { get; }

		public bool IsProgressViewExpanded { get; }
		public bool IsNetworkViewExpanded { get; }

		public bool IsBoardRotated { get; }

		public string ProgressCaption { get; }
		public string NetworkViewCaption { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}