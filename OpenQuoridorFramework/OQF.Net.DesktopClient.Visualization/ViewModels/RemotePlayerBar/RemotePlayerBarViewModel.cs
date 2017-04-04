using System.ComponentModel;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using OQF.Net.DesktopClient.Contracts;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar
{
	public class RemotePlayerBarViewModel : ViewModel, IRemotePlayerBarViewModel
	{
		private readonly INetworkGameService networkGameService;		
		private bool? isGameInitiator;
		private string wallsLeft;
		private string playerName;
		private string playerStatus;

		public RemotePlayerBarViewModel (INetworkGameService networkGameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.networkGameService = networkGameService;

			networkGameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			networkGameService.GameOver               += OnGameOver;
			networkGameService.GameStatusChanged      += OnGameStatusChanged;

			OnNewBoardStateAvailable(networkGameService.CurrentBoardState);			
		}

		private void OnGameStatusChanged (GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				switch (newGameStatus)
				{
					case GameStatus.PlayingJoinedGame:
					{
						IsGameInitiator = true;
						PlayerName = networkGameService.OpponendPlayer.Name;
						break;
					}
					case GameStatus.PlayingOpendGame:
					{
						IsGameInitiator = false;
						PlayerName = networkGameService.OpponendPlayer.Name;
						break;
					}
					case GameStatus.NoGame:
					{
						OnGameOver(false, WinningReason.Capitulation);
						break;
					}
				}
			});
		}

		private void OnGameOver (bool arg1, WinningReason arg2)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{				
				WallsLeft = "--";
				IsGameInitiator = null;
				PlayerName = "--";
				PlayerStatus = string.Empty;
			});
		}

		private void OnNewBoardStateAvailable (BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (boardState != null && IsGameInitiator.HasValue)
				{
					WallsLeft = IsGameInitiator.Value
									? boardState.BottomPlayer.WallsToPlace.ToString()
									: boardState.TopPlayer.WallsToPlace.ToString();

					if (networkGameService.ClientPlayer != null &&
					    boardState.CurrentMover.PlayerType == networkGameService.OpponendPlayer.PlayerType)
					{
						PlayerStatus = Captions.NCl_RemotePlayerStatus;
					}
					else
					{
						PlayerStatus = string.Empty;
					}
				}
				else
				{
					WallsLeft = "--";
					IsGameInitiator = null;
					PlayerName = "--";
				}

			});
		}				

		public bool? IsGameInitiator
		{
			get { return isGameInitiator; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isGameInitiator, value); }
		}

		public string WallsLeft
		{
			get { return wallsLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref wallsLeft, value); }
		}

		public string PlayerName
		{
			get { return playerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref playerName, value); }
		}

		public string PlayerStatus
		{
			get { return playerStatus; }
			private set { PropertyChanged.ChangeAndNotify(this, ref playerStatus, value); }
		}

		public string WallsLeftLabelCaption => Captions.PvB_WallsLeftLabelCaption;		

		private void RefreshCaptions ()
		{
			if (!string.IsNullOrEmpty(PlayerStatus))
				PlayerStatus = Captions.NCl_RemotePlayerStatus;

			PropertyChanged.Notify(this, nameof(WallsLeftLabelCaption));
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			networkGameService.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			networkGameService.GameStatusChanged      -= OnGameStatusChanged;
			networkGameService.GameOver               -= OnGameOver;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}