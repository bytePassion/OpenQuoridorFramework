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
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView
{
	public class NetworkViewModel : ViewModel, INetworkViewModel
	{
		private readonly INetworkGameService networkGameService;
		private string response;
		private string serverAddress;
		private string playerName;
		private ConnectionStatus connectionStatus;

		public NetworkViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;

			networkGameService.JoinError += OnJoinError;
			networkGameService.JoinSuccessful += NetworkGameServiceOnJoinSuccessful;
			networkGameService.OpendGameIsStarting += OnOpendGameIsStarting;
			networkGameService.GameOver += OnGameOver;
			networkGameService.ConnectionStatusChanged += OnConnectionStatusChanged;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;
			

			AvailableOpenGames = new ObservableCollection<GameDisplayData>();

			ConnectToServer = new Command(DoConnect,
										  IsConnectPossible,
										  new PropertyChangedCommandUpdater(this, nameof(PlayerName),
																				  nameof(ServerAddress),
																				  nameof(ConnectionStatus)));

			CreateGame = new Command(DoCreateGame);
			JoinGame = new Command(DoJoinGame);
		}

		private bool IsConnectPossible()
		{
			return ConnectionStatus == ConnectionStatus.NotConnected &&
			       !string.IsNullOrWhiteSpace(PlayerName) &&
			       AddressIdentifier.IsIpAddressIdentifier(ServerAddress);
		}

		private void OnConnectionStatusChanged(ConnectionStatus newConnectionStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = $"ConnectionStatus: {newConnectionStatus}";
				ConnectionStatus = newConnectionStatus;
			});
		}
	
		public ICommand ConnectToServer { get; }
		public ICommand CreateGame { get; }
		public ICommand JoinGame { get; }
		public string NewGameName { get; set; }

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

		public string PlayerName
		{
			get { return playerName; }
			set { PropertyChanged.ChangeAndNotify(this, ref playerName, value); }
		}

		public string Response
		{
			get { return response; }
			private set { PropertyChanged.ChangeAndNotify(this, ref response, value); }
		}

		private void OnUpdatedGameListAvailable (IDictionary<NetworkGameId, string> newGameList)
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

		private void OnGameOver (bool b, WinningReason winningReason)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var msg = b ? "won" : "lost";
				Response = $"game is {msg} because {winningReason}";
			});
		}

		private void OnOpendGameIsStarting (string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = $"game is starting with {s}";
			});
		}

		private void NetworkGameServiceOnJoinSuccessful (string s)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = $"join successful with {s}";
			});
		}

		private void OnJoinError ()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = "join error";
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

		public ObservableCollection<GameDisplayData> AvailableOpenGames { get; }

		public GameDisplayData SelectedOpenGame { get; set; }

		protected override void CleanUp ()
		{
			// TODO
		}
		public override event PropertyChangedEventHandler PropertyChanged;

	}
}
