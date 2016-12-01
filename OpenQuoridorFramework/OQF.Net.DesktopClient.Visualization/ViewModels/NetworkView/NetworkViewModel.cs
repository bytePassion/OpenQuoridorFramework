using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
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

		public NetworkViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;

			networkGameService.JoinError += OnJoinError;
			networkGameService.JoinSuccessful += NetworkGameServiceOnJoinSuccessful;
			networkGameService.OpendGameIsStarting += OnOpendGameIsStarting;
			networkGameService.GameOver += OnGameOver;
			networkGameService.GotConnected += OnGotConnected;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;

			AvailableOpenGames = new ObservableCollection<GameDisplayData>();

			ConnectToServer = new Command(DoConnect);
			CreateGame = new Command(DoCreateGame);
			JoinGame = new Command(DoJoinGame);
		}


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

		private void OnGotConnected ()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Response = "positive";
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
			
		}
		public override event PropertyChangedEventHandler PropertyChanged;

	}
}
