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


namespace OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow
{
	public class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly INetworkGameService networkGameService;
		private string response;

		public MainWindowViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;
			networkGameService.GotConnected += OnGotConnected;
			networkGameService.UpdatedGameListAvailable += OnUpdatedGameListAvailable;

			AvailableOpenGames = new ObservableCollection<GameDisplayData>();

			ConnectToServer = new Command(DoConnect);
			CreateGame = new Command(DoCreateGame);
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

		public ICommand ConnectToServer { get; }
		public ICommand CreateGame { get; }
		public string NewGameName { get; set; }

		public string ServerAddress { get; set; }
		public string PlayerName { get; set; }

		public string Response
		{
			get { return response; }
			private set { PropertyChanged.ChangeAndNotify(this, ref response, value); }
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

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
