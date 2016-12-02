using System;
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
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow.Helper;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;

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

				SelectedOpenGame = AvailableOpenGames.FirstOrDefault();

				// TODO: restore selection;
			});
		}
		
		private void DoConnect ()
		{
			networkGameService.ConnectToServer(AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress), PlayerName);
		}

		private void DoCreateGame ()
		{
			var newGameId = new NetworkGameId( Guid.NewGuid());
			networkGameService.CreateGame(NewGameName, newGameId);
		}

		private void DoJoinGame ()
		{
			networkGameService.JoinGame(SelectedOpenGame.GameId, SelectedOpenGame.GameName);
		}

		private void DoLeaveGame()
		{
			// TODO: ask user
			networkGameService.LeaveGame();
		}

		private void DoCancelCreatedGame()
		{
			networkGameService.CancelCreatedGame();
		}

		private void DoDisconnectFromServer()
		{
			// TODO: ask user
			networkGameService.Disconnect();
		}

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }		

		protected override void CleanUp ()
		{
			networkGameService.ConnectionStatusChanged  += OnConnectionStatusChanged;
			networkGameService.GameStatusChanged        += OnGameStatusChanged;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;

	}
}
