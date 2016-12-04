using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView
{
	public class NetworkViewModel : ViewModel, INetworkViewModel
	{
		private readonly INetworkGameService networkGameService;
		private readonly IApplicationSettingsRepository applicationSettingsRepository;		
		private string serverAddress;
		private string playerName;
		private ConnectionStatus connectionStatus;
		private GameStatus gameStatus;
		private string newGameName;
		private GameDisplayData selectedOpenGame;

		public NetworkViewModel(INetworkGameService networkGameService, IApplicationSettingsRepository applicationSettingsRepository)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.networkGameService = networkGameService;
			this.applicationSettingsRepository = applicationSettingsRepository;

			networkGameService.ConnectionStatusChanged  += OnConnectionStatusChanged;
			networkGameService.GameStatusChanged        += OnGameStatusChanged;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;
			
			AvailableOpenGames = new ObservableCollection<GameDisplayData>();

			ConnectToServer = new Command(DoConnect,
										  () => ConnectionStatus == ConnectionStatus.NotConnected &&
												!string.IsNullOrWhiteSpace(PlayerName) &&
												AddressIdentifier.IsIpAddressIdentifier(ServerAddress),
										  new PropertyChangedCommandUpdater(this, nameof(PlayerName),
																				  nameof(ServerAddress),
																				  nameof(ConnectionStatus)));

			CreateGame = new Command(DoCreateGame,
									 () => !string.IsNullOrWhiteSpace(NewGameName) && 
										   (GameStatus == GameStatus.GameOver || GameStatus == GameStatus.NoGame),
									 new PropertyChangedCommandUpdater(this, nameof(NewGameName), nameof(GameStatus)));

			JoinGame = new Command(DoJoinGame,
								   () => SelectedOpenGame != null &&
										 (GameStatus == GameStatus.GameOver || GameStatus == GameStatus.NoGame),
								   new PropertyChangedCommandUpdater(this, nameof(SelectedOpenGame), nameof(GameStatus)));

			DisconnectFromServer = new Command(DoDisconnectFromServer,
											   () => ConnectionStatus == ConnectionStatus.Connected,
											   new PropertyChangedCommandUpdater(this, nameof(ConnectionStatus)));

			CancelCreatedGame = new Command(DoCancelCreatedGame,
											() => GameStatus == GameStatus.WaitingForOponend,
											new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			LeaveGame = new Command(DoLeaveGame,
									() => GameStatus == GameStatus.PlayingJoinedGame || GameStatus == GameStatus.PlayingOpendGame,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			ServerAddress = applicationSettingsRepository.LastConnectedServerAddress;
			PlayerName = applicationSettingsRepository.LastUsedPlayerName;
		}		

		private void OnGameStatusChanged(GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{				
				GameStatus = newGameStatus;
			});
		}

		private void OnConnectionStatusChanged(ConnectionStatus newConnectionStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (newConnectionStatus == ConnectionStatus.Connected)
				{
					applicationSettingsRepository.LastConnectedServerAddress = ServerAddress;
					applicationSettingsRepository.LastUsedPlayerName = PlayerName;
				}

				ConnectionStatus = newConnectionStatus;
			});
		}

		
		public ICommand ConnectToServer      { get; }
		public ICommand DisconnectFromServer { get; }
		public ICommand CreateGame           { get; }
		public ICommand CancelCreatedGame    { get; }
		public ICommand JoinGame             { get; }
		public ICommand LeaveGame            { get; }
		


		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gameStatus, value); }
		}

		public ConnectionStatus ConnectionStatus
		{
			get { return connectionStatus; }
			private set {PropertyChanged.ChangeAndNotify(this, ref connectionStatus, value); }
		}

		public string ServerAddress
		{
			get { return serverAddress; }
			set { PropertyChanged.ChangeAndNotify(this, ref serverAddress, value); }
		}

		public string NewGameName
		{
			get { return newGameName; }
			set { PropertyChanged.ChangeAndNotify(this, ref newGameName, value); }
		}

		public string PlayerName
		{
			get { return playerName; }
			set { PropertyChanged.ChangeAndNotify(this, ref playerName, value); }
		}

		public GameDisplayData SelectedOpenGame
		{
			get { return selectedOpenGame; }
			set { PropertyChanged.ChangeAndNotify(this, ref selectedOpenGame, value); }
		}

		private void OnUpdatedGameListAvailable (IEnumerable<NetworkGameInfo> newGameList)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				AvailableOpenGames.Clear();

				foreach (var openGame in newGameList)
				{
					AvailableOpenGames.Add(new GameDisplayData(openGame.GameId, openGame.GameName, openGame.InitiatorName));
				}

				if (SelectedOpenGame != null)
				{
					var lastSelection = AvailableOpenGames.FirstOrDefault(game => game.GameId == SelectedOpenGame.GameId);

					SelectedOpenGame = lastSelection ?? AvailableOpenGames.FirstOrDefault();
				}
				else
				{
					SelectedOpenGame = AvailableOpenGames.FirstOrDefault();
				}												
			});
		}
		
		private void DoConnect ()
		{
			networkGameService.ConnectToServer(AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress), PlayerName);
		}

		private void DoCreateGame ()
		{			
			networkGameService.CreateGame(NewGameName);
		}

		private void DoJoinGame ()
		{
			networkGameService.JoinGame(SelectedOpenGame.GameId, SelectedOpenGame.GameName);
		}

		private async void DoLeaveGame()
		{
			var userConfirmLeaving = await YesNoDialogService.Show(Captions.NCl_Confirmation_LeaveGame);

			if (userConfirmLeaving)
				networkGameService.LeaveGame();
		}

		private async void DoCancelCreatedGame()
		{
			var userConfirmLeaving = await YesNoDialogService.Show(Captions.NCl_Confirmation_CancelGame);

			if (userConfirmLeaving)
				networkGameService.CancelCreatedGame();
		}

		private async void DoDisconnectFromServer()
		{
			var userConfirmLeaving = await YesNoDialogService.Show(Captions.NCl_Confirmation_Disconnect);

			if (userConfirmLeaving)
				networkGameService.Disconnect();
		}

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }		

		public string ServerAddressPromt                  => Captions.NCl_ServerAddressPromt;
		public string ServerAddressHint                   => Captions.NCl_ServerAddressHint;
		public string PlayerNamePromt                     => Captions.NCl_PlayerNamePromt;
		public string PlayerNameHint                      => Captions.NCl_PlayerNameHint;
		public string ConnectToServerButtonCaption        => Captions.NCl_ConnectToServerButtonCaption;
		public string DisconnectFromServerButtonsCaptions => Captions.NCl_DisconectFromServerButtonCaption;
		public string NewGameNamePromt                    => Captions.NCl_NewGameNamePromt;
		public string NewGameNameHint                     => Captions.NCl_NewGameNameHint;
		public string OpenGameListSectionHeader           => Captions.NCl_OpenGameListSectionHeader;
		public string JoinGameButtonCaption               => Captions.NCl_JoinGameButtonCaption;
		public string CancelCreatedGameButtonCaption      => Captions.NCl_CancelCreatedGameButtonCaption;
		public string LeaveGameButtonCaption              => Captions.NCl_LeaveGameButtonCaption;


		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(ServerAddressPromt),
										 nameof(ServerAddressHint),
										 nameof(PlayerNamePromt),
										 nameof(PlayerNameHint),
										 nameof(ConnectToServerButtonCaption),
										 nameof(DisconnectFromServerButtonsCaptions),
										 nameof(NewGameNamePromt),
										 nameof(NewGameNameHint),
										 nameof(OpenGameListSectionHeader),
										 nameof(JoinGameButtonCaption),
										 nameof(CancelCreatedGameButtonCaption),
										 nameof(LeaveGameButtonCaption),
										 nameof(ConnectionStatus));
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			networkGameService.ConnectionStatusChanged  += OnConnectionStatusChanged;
			networkGameService.GameStatusChanged        += OnGameStatusChanged;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;

	}
}
