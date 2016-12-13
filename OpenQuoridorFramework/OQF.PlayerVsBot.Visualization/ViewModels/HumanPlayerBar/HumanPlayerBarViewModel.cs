using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.Commands.Updater;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.Global;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.PlayerVsBot.Visualization.ViewModels.HumanPlayerBar
{
	public class HumanPlayerBarViewModel : ViewModel, IHumanPlayerBarViewModel
	{
		private readonly IGameService gameService;

		private int bottomPlayerWallCountLeft;
		private string movesLeft;
		private GameStatus gameStatus;

		public HumanPlayerBarViewModel(IGameService gameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;

			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameService.NewGameStatusAvailable += OnNewGameStatusAvailable;

			MovesLeft = "--";

			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
		}

		private void OnNewGameStatusAvailable(GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				GameStatus = newGameStatus;

				if (newGameStatus == GameStatus.Active)
					MovesLeft = (Constants.GameConstraint.MaximalMovesPerGame + 1).ToString();
			});			
		}

		private void OnNewBoardStateAvailable (BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				((Command)Capitulate).RaiseCanExecuteChanged();

				if (boardState == null)
				{					
					BottomPlayerWallCountLeft = 10;
					MovesLeft = "--";				
				}
				else
				{					
					BottomPlayerWallCountLeft = boardState.BottomPlayer.WallsToPlace;

					if (boardState.CurrentMover.PlayerType == gameService.HumanPlayerPosition)
					{						
						var currentMovesLeft = int.Parse(MovesLeft);
						MovesLeft = (currentMovesLeft - 1).ToString();
					}					
				}
			});
		}

		public ICommand Capitulate { get; }

		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gameStatus, value); }
		}

		public int BottomPlayerWallCountLeft
		{
			get { return bottomPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref bottomPlayerWallCountLeft, value); }
		}

		public string MovesLeft
		{
			get { return movesLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref movesLeft, value); }
		}

		private bool IsMoveApplyable ()
		{
			if (gameService.CurrentGameStatus != GameStatus.Active)
				return false;

			return gameService.CurrentBoardState?.CurrentMover.PlayerType == gameService.HumanPlayerPosition;
		}

		private void DoCapitulate ()
		{
			gameService.ReportHumanMove(new Capitulation());
		}

		public string WallsLeftLabelCaption   => Captions.PvB_WallsLeftLabelCaption;
		public string CapitulateButtonCaption => Captions.PvB_CapitulateButtonCaption;
		public string MovesLeftLabelCaption   => Captions.PvB_MovesLeftLabelCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(WallsLeftLabelCaption),										 
										 nameof(CapitulateButtonCaption),										 
										 nameof(MovesLeftLabelCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
