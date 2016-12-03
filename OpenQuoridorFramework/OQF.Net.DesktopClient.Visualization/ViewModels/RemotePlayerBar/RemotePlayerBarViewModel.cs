using System.ComponentModel;
using System.Windows;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
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
						break;
					}
					case GameStatus.PlayingOpendGame:
					{
						IsGameInitiator = false;
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
				}
				else
				{
					WallsLeft = "--";
					IsGameInitiator = null;
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

		public string WallsLeftLabelCaption => Captions.PvB_WallsLeftLabelCaption;		

		private void RefreshCaptions ()
		{
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