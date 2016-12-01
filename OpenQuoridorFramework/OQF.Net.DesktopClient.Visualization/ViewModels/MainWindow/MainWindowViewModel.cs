using System.ComponentModel;
using System.Windows;
using Lib.Communication.State;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;
using OQF.Utils.Enum;


namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		
		private readonly ISharedStateReadOnly<bool> isBoardRotatedVariable;
		
		private bool isBoardRotated;

		public MainWindowViewModel(INetworkGameService networkGameService,
								   ISharedStateReadOnly<bool> isBoardRotatedVariable,
								   IBoardPlacementViewModel boardPlacementViewModel, 
								   IBoardViewModel boardViewModel, 
								   IProgressViewModel progressViewModel, 
								   IActionBarViewModel actionBarViewModel, 
								   IBoardLabelingViewModel boardHorizontalLabelingViewModel, 
								   IBoardLabelingViewModel boardVerticalLabelingViewModel, 
								   ILocalPlayerBarViewModel localPlayerBarViewModel, 
								   IRemotePlayerBarViewModel remotePlayerBarViewModel, 
								   INetworkViewModel networkViewModel)
		{			
			this.isBoardRotatedVariable = isBoardRotatedVariable;
			BoardPlacementViewModel = boardPlacementViewModel;
			BoardViewModel = boardViewModel;
			ProgressViewModel = progressViewModel;
			ActionBarViewModel = actionBarViewModel;
			BoardHorizontalLabelingViewModel = boardHorizontalLabelingViewModel;
			BoardVerticalLabelingViewModel = boardVerticalLabelingViewModel;
			LocalPlayerBarViewModel = localPlayerBarViewModel;
			RemotePlayerBarViewModel = remotePlayerBarViewModel;
			NetworkViewModel = networkViewModel;

			networkGameService.GameOver += OnGameOver;

			isBoardRotatedVariable.StateChanged += OnIsBoardRotatedVariableChanged;
			OnIsBoardRotatedVariableChanged(isBoardRotatedVariable.Value);						
		}

		private void OnGameOver(bool b, WinningReason winningReason)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var msg = b ? "winner" : "looser";
				MessageBox.Show($"{msg} because {winningReason}");
			});			
		}

		private void OnIsBoardRotatedVariableChanged(bool newIsBoardRotated)
		{
			IsBoardRotated = newIsBoardRotated;
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

		public bool IsBoardRotated
		{
			get { return isBoardRotated; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBoardRotated, value); }
		}
		
		protected override void CleanUp()
		{
			isBoardRotatedVariable.StateChanged -= OnIsBoardRotatedVariableChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
