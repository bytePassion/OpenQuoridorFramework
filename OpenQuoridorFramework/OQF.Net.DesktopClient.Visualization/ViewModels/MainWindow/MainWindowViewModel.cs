using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.Communication.State;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;


namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameService networkGameService;
		private readonly ISharedStateReadOnly<bool> isBoardRotatedVariable;
		private string response;
		private bool isBoardRotated;

		public MainWindowViewModel(INetworkGameService networkGameService, 
								   ISharedStateReadOnly<bool> isBoardRotatedVariable,
								   IBoardPlacementViewModel boardPlacementViewModel, 
								   IBoardViewModel boardViewModel, 
								   IProgressViewModel progressViewModel, 
								   IActionBarViewModel actionBarViewModel, 
								   IBoardLabelingViewModel boardHorizontalLabelingViewModel, 
								   IBoardLabelingViewModel boardVerticalLabelingViewModel, ILocalPlayerBarViewModel localPlayerBarViewModel, IRemotePlayerBarViewModel remotePlayerBarViewModel)
		{
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

			networkGameService.GotConnected += OnGotConnected;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;
			networkGameService.JoinError += OnJoinError;
			networkGameService.JoinSuccessful += NetworkGameServiceOnJoinSuccessful;
			networkGameService.OpendGameIsStarting += OnOpendGameIsStarting;
			networkGameService.GameOver += OnGameOver;

			isBoardRotatedVariable.StateChanged += OnIsBoardRotatedVariableChanged;
			OnIsBoardRotatedVariableChanged(isBoardRotatedVariable.Value);

			AvailableOpenGames = new ObservableCollection<GameDisplayData>();

			ConnectToServer = new Command(DoConnect);
			CreateGame = new Command(DoCreateGame);
			JoinGame = new Command(DoJoinGame);
		}

		private void OnIsBoardRotatedVariableChanged(bool newIsBoardRotated)
		{
			IsBoardRotated = newIsBoardRotated;
		}

		private void OnGameOver(bool b, WinningReason winningReason)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var msg = b ? "won" : "lost";
				Response = $"game is {msg} because {winningReason}";
			});
		}

		private void OnOpendGameIsStarting(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = $"game is starting with {s}";
			});
		}

		private void NetworkGameServiceOnJoinSuccessful(string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = $"join successful with {s}";
			});			
		}

		private void OnJoinError()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = "join error";
			});
		}

		private void OnUpdatedGameListAvailable(IDictionary<NetworkGameId, string> newGameList)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				AvailableOpenGames.Clear();

				foreach (var openGame in newGameList)
				{
					AvailableOpenGames.Add(new GameDisplayData(openGame.Key, openGame.Value));
				}

				SelectedOpenGame = AvailableOpenGames.FirstOrDefault();

				// TODO: restore selection;
			});			
		}

		private void OnGotConnected()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = "positive";
			});
		}

		public IBoardPlacementViewModel BoardPlacementViewModel { get; }
		public IBoardViewModel BoardViewModel { get; }
		public IProgressViewModel ProgressViewModel { get; }
		public IActionBarViewModel ActionBarViewModel { get; }
		public IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
		public IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }
		public ILocalPlayerBarViewModel LocalPlayerBarViewModel { get; }
		public IRemotePlayerBarViewModel RemotePlayerBarViewModel { get; }

		public ICommand ConnectToServer { get; }
		public ICommand CreateGame { get; }
		public ICommand JoinGame { get; }
		public string NewGameName { get; set; }

		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }

		public string Response
		{
			get { return response; }
			private set { PropertyChanged.ChangeAndNotify(this, ref response, value); }
		}

		public bool IsBoardRotated
		{
			get { return isBoardRotated; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isBoardRotated, value); }
		}

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }

		public GameDisplayData SelectedOpenGame { get; set; }

		private void DoConnect()
		{
			networkGameService.ConnectToServer(AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress), PlayerName);
		}

		private void DoCreateGame()
		{
			var newGameId = new NetworkGameId( Guid.NewGuid());
			networkGameService.CreateGame(NewGameName, newGameId);	
		}

		private void DoJoinGame()
		{
			networkGameService.JoinGame(SelectedOpenGame.GameId, SelectedOpenGame.GameName);
		}

		protected override void CleanUp()
		{
			isBoardRotatedVariable.StateChanged -= OnIsBoardRotatedVariableChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
