using System.ComponentModel;
using System.Text;
using System.Windows;
using Lib.Communication.State;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Dialogs.Notification;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;
using OQF.Utils.Enum;


namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameService networkGameService;
		private readonly ISharedStateReadOnly<bool> isBoardRotatedVariable;
		
		private bool isBoardRotated;
		private bool isProgressViewExpanded;
		private bool isNetworkViewExpanded;

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
			CultureManager.CultureChanged += RefreshCaptions;

			this.networkGameService = networkGameService;
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
			networkGameService.GameStatusChanged += OnGameStatusChanged;

			OnGameStatusChanged(networkGameService.CurrentGameStatus);

			isBoardRotatedVariable.StateChanged += OnIsBoardRotatedVariableChanged;
			OnIsBoardRotatedVariableChanged(isBoardRotatedVariable.Value);						
		}		

		private void OnGameStatusChanged(GameStatus gameStatus)
		{
			switch (gameStatus)
			{
				case GameStatus.NoGame:								
				case GameStatus.WaitingForOponend:
				{
					IsNetworkViewExpanded = true;
					IsProgressViewExpanded = false;
					break;
				}
				case GameStatus.PlayingJoinedGame:
				case GameStatus.PlayingOpendGame:
				{
					IsNetworkViewExpanded = false;
					IsProgressViewExpanded = true;
					break;
				}
				case GameStatus.GameOver:
				{
					IsNetworkViewExpanded = true;
					IsProgressViewExpanded = true;
					break;
				}
			}
		}

		private void OnGameOver(bool b, WinningReason winningReason)
		{
			Application.Current.Dispatcher.InvokeAsync(async () =>
			{
				await NotificationDialogService.Show(GetWinningOrLoosingMessage(b, winningReason), "Ok");
			});			
		}


		private static string GetWinningOrLoosingMessage (bool reportWinning, WinningReason winningReason)
		{
			var sb = new StringBuilder();

			sb.Append(reportWinning
							? $"{Captions.NCl_WinningMessage}"
							: $"{Captions.NCL_LoosingMessage}");

			sb.Append($"\n{Captions.WD_Message_Reason}: {WinningReasonToString(winningReason)}");						

			return sb.ToString();
		}

		private static string WinningReasonToString(WinningReason winningReason)
	    {
		    switch (winningReason)
		    {
			    case WinningReason.Capitulation:            return Captions.WinningReason_Capitulation;				
				case WinningReason.InvalidMove:             return Captions.WinningReason_InvalidMode;
				case WinningReason.ExceedanceOfMaxMoves:    return Captions.WinningReason_ExceedanceOfMaxMoves;
				case WinningReason.ExceedanceOfThoughtTime: return Captions.WinningReason_ExceedanceOfThoughtTime;
				case WinningReason.RegularQuoridorWin:      return Captions.WinningReason_RegularQuoridorWin;			    
		    }

		    return "";
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

		public bool IsProgressViewExpanded
		{
			get { return isProgressViewExpanded; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isProgressViewExpanded, value); }
		}

		public bool IsNetworkViewExpanded
		{
			get { return isNetworkViewExpanded; }
			private set {PropertyChanged.ChangeAndNotify(this, ref isNetworkViewExpanded, value); }
		}

		public bool IsBoardRotated
		{
			get { return isBoardRotated; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBoardRotated, value); }
		}

		public string ProgressCaption => Captions.PvB_ProgressCaption;
		public string NetworkViewCaption => Captions.NCl_NetworkViewCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(ProgressCaption), 
										 nameof(NetworkViewCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			networkGameService.GameOver -= OnGameOver;
			networkGameService.GameStatusChanged -= OnGameStatusChanged;

			isBoardRotatedVariable.StateChanged -= OnIsBoardRotatedVariableChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
