using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;
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

		ICommand ConnectToServer { get; }
		ICommand CreateGame { get; }
		ICommand JoinGame { get; }

		string NewGameName { get; set; }
		string ServerAddress { get; set; }
		string PlayerName { get; set; }

		string Response { get; }

		bool IsBoardRotated { get; }

		ObservableCollection<GameDisplayData> AvailableOpenGames { get; }
		GameDisplayData SelectedOpenGame { get; set; }
	}
}