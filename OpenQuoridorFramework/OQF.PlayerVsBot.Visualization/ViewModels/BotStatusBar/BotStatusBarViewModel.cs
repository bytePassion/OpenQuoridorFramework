using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Lib.FrameworkExtension;
using Lib.Utils;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using OQF.PlayerVsBot.Contracts;
using OQF.PlayerVsBot.Visualization.Global;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.PlayerVsBot.Visualization.ViewModels.BotStatusBar
{
	public class BotStatusBarViewModel : ViewModel, IBotStatusBarViewModel
	{
		private readonly IGameService gameService;
		private readonly Timer botCountDownTimer;
		private DateTime startTime;

		private int topPlayerWallCountLeft;		
		private string topPlayerRestTime;
		private GameStatus gameStatus;

		public BotStatusBarViewModel(IGameService gameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			this.gameService = gameService;

			gameService.NewGameStatusAvailable += OnNewGameStatusAvailable;
			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;			

			botCountDownTimer = new Timer(BotCountDownTimerOnTick, null, Timeout.Infinite, Timeout.Infinite);
			StopTimer();

			GameStatus = gameService.CurrentGameStatus;
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			if (boardState != null)
			{
				Application.Current?.Dispatcher.Invoke(() =>
				{					
					TopPlayerWallCountLeft = boardState.TopPlayer.WallsToPlace;

					if (boardState.CurrentMover.PlayerType == gameService.HumanPlayerPosition)
					{
						StopTimer();
					}
					else
					{
						StartTimer();
					}
				});				
			}
		}

		private void OnNewGameStatusAvailable(GameStatus newGameStatus)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				GameStatus = newGameStatus;

				switch (newGameStatus)
				{
					case GameStatus.Finished:
					{
						StopTimer();
						break;
					}
					case GameStatus.Unloaded:
					{
						TopPlayerWallCountLeft = 10;
						TopPlayerRestTime = "--";
						break;
					}
				}
			});			
		}

		private void BotCountDownTimerOnTick (object sender)
		{
			var timeDiff = DateTime.Now - startTime;

			Application.Current?.Dispatcher.Invoke(() =>
			{
				TopPlayerRestTime = GeometryLibUtils.DoubleFormat(Constants.GameConstraint.BotThinkingTimeSeconds - timeDiff.TotalSeconds, 2);
			});
		}


		private void StartTimer ()
		{
			startTime = DateTime.Now;
			TopPlayerRestTime = Constants.GameConstraint.BotThinkingTimeSeconds.ToString();
			botCountDownTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
		}

		private void StopTimer ()
		{
			startTime = default(DateTime);
			botCountDownTimer.Change(Timeout.Infinite, Timeout.Infinite);
			TopPlayerRestTime = "--";
		}


		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gameStatus, value); }
		}
		
		public string TopPlayerRestTime
		{
			get { return topPlayerRestTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerRestTime, value); }
		}

		public int TopPlayerWallCountLeft
		{
			get { return topPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerWallCountLeft, value); }
		}

		public string BotNameLabelCaption             => Captions.PvB_BotNameLabelCaption;
		public string MaximalThinkingTimeLabelCaption => Captions.PvB_MaximalThinkingTimeLabelCaption;
		public string WallsLeftLabelCaption           => Captions.PvB_WallsLeftLabelCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(BotNameLabelCaption),
										 nameof(MaximalThinkingTimeLabelCaption),
										 nameof(WallsLeftLabelCaption));
		}

		protected override void CleanUp()
		{
			gameService.NewGameStatusAvailable -= OnNewGameStatusAvailable;
			gameService.NewBoardStateAvailable -= OnNewBoardStateAvailable;

			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
