using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.Net.DesktopClient.Contracts;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	public class LocalPlayerBarViewModel : ViewModel, ILocalPlayerBarViewModel
	{
		private readonly INetworkGameService networkGameService;
		private bool isPlacementPossible;
		private bool? isGameInitiator;
		private string wallsLeft;
		private string playerName;

		public LocalPlayerBarViewModel(INetworkGameService networkGameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.networkGameService = networkGameService;

			networkGameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			networkGameService.GameOver               += OnGameOver;
			networkGameService.GameStatusChanged      += OnGameStatusChanged;

			OnNewBoardStateAvailable(networkGameService.CurrentBoardState);

			Capitulate = new Command(DoCapitulate, 
									 () => IsPlacementPossible, 
									 new PropertyChangedCommandUpdater(this, nameof(IsPlacementPossible)));
		}
			
		private void OnGameStatusChanged(GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				switch (newGameStatus)
				{
					case GameStatus.PlayingJoinedGame:
					{
						IsGameInitiator = false;
						PlayerName = networkGameService.ClientPlayer.Name;
						break;
					}
					case GameStatus.PlayingOpendGame:
					{
						IsGameInitiator = true;
						PlayerName = networkGameService.ClientPlayer.Name;
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
		
		private async void DoCapitulate()
		{
			var userConfirmLeaving = await YesNoDialogService.Show("wirklich kapitulieren?");

			if (userConfirmLeaving)
				networkGameService.SubmitMove(new Capitulation());
		}

		private void OnGameOver(bool arg1, WinningReason arg2)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				IsPlacementPossible = false;
				WallsLeft = "--";
				IsGameInitiator = null;
				PlayerName = "--";
			});
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				IsPlacementPossible = boardState != null && 
									  networkGameService.ClientPlayer != null &&
				                      boardState.CurrentMover.PlayerType == networkGameService.ClientPlayer.PlayerType;

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
					PlayerName = "--";
				}

			});
		}

		private bool IsPlacementPossible
		{
			get { return isPlacementPossible; }
			set { PropertyChanged.ChangeAndNotify(this, ref isPlacementPossible, value); }
		}

		public ICommand Capitulate { get; }

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

		public string WallsLeftLabelCaption   => Captions.PvB_WallsLeftLabelCaption;
		public string CapitulateButtonCaption => Captions.PvB_CapitulateButtonCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(WallsLeftLabelCaption), 
										 nameof(CapitulateButtonCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			networkGameService.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			networkGameService.GameOver               -= OnGameOver;
			networkGameService.GameStatusChanged      -= OnGameStatusChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
