using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;
using QCF.GameEngine.Contracts;
using QCF.SingleGameVisualization.Services;
using QCF.SingleGameVisualization.Tools;
using QCF.SingleGameVisualization.ViewModels.AboutHelpWindow;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.SingleGameVisualization.ViewModels.BoardPlacement;
using QCF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using QCF.Tools.FrameworkExtensions;
using QCF.Tools.WpfTools.Commands;
using QCF.Tools.WpfTools.Commands.Updater;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly DispatcherTimer botCountDownTimer;

		private readonly IGameService gameService;
		private readonly ILastUsedBotService lastUsedBotService;

		private string dllPathInput;
		private string moveInput;
		private int bottomPlayerWallCountLeft;
		private int topPlayerWallCountLeft;		
		private string topPlayerName;
		private GameStatus gameStatus;
		private bool isAutoScrollProgressActive;
		private bool isAutoScrollDebugMsgActive;
		private string topPlayerRestTime;
		private bool isDisabledOverlayVisible;

		public MainWindowViewModel (IBoardViewModel boardViewModel, 
									IBoardPlacementViewModel boardPlacementViewModel, 
									IGameService gameService, 
									ILastUsedBotService lastUsedBotService)
		{
			this.gameService = gameService;
			this.lastUsedBotService = lastUsedBotService;
			BoardPlacementViewModel = boardPlacementViewModel;
			BoardViewModel = boardViewModel;
			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();			
			
			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameService.WinnerAvailable        += OnWinnerAvailable;
			gameService.NewDebugMsgAvailable   += OnNewDebugMsgAvailable;

			IsAutoScrollDebugMsgActive = true;
			IsAutoScrollProgressActive = true;

			BrowseDll = new Command(DoBrowseDll);
			Start = new Command(DoStart,
								() => GameStatus != GameStatus.Active,
								new PropertyChangedCommandUpdater(this, nameof(GameStatus)));				
			ApplyMove = new Command(DoApplyMove,
									IsMoveApplyable,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			ShowAboutHelp = new Command(DoShowAboutHelp);

			GameStatus = GameStatus.Unloaded;

			DllPathInput = lastUsedBotService.GetLastUsedBot();

			botCountDownTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1),
				IsEnabled = false
			};

			botCountDownTimer.Tick += BotCountDownTimerOnTick;
		}		

		private void BotCountDownTimerOnTick(object sender, EventArgs eventArgs)
		{
			var currentCount = int.Parse(TopPlayerRestTime);
			TopPlayerRestTime = (currentCount - 1).ToString();
		}

		private void DoShowAboutHelp()
		{
			var aboutHelpWindowViewModel = new AboutHelpWindowViewModel();

			var window = new Windows.AboutHelpWindow()
			{
				Owner = Application.Current.MainWindow,
				DataContext = aboutHelpWindowViewModel
			};

			window.ShowDialog();
		}

		private void OnNewDebugMsgAvailable(string s)
		{
			DebugMessages.Add(s);
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason)
		{
			MessageBox.Show(
				player.PlayerType == PlayerType.TopPlayer
						? $"Sry ... the bot '{player.Name}' beated you\nReason: {winningReason}"
						: $"congratulations! you beated the bot\nReason: {winningReason}"
			);

			GameStatus = GameStatus.Finished;
			StopTimer();
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			((Command)ApplyMove).RaiseCanExecuteChanged();
			((Command)Capitulate).RaiseCanExecuteChanged();

			if (boardState == null)
			{
				GameStatus = GameStatus.Unloaded;

				TopPlayerName = string.Empty;
				TopPlayerWallCountLeft = 10;
				BottomPlayerWallCountLeft = 10;

				GameProgress.Clear();
			}
			else
			{
				GameStatus = GameStatus.Active;

				TopPlayerName = boardState.TopPlayer.Player.Name;

				TopPlayerWallCountLeft = boardState.TopPlayer.WallsToPlace;
				BottomPlayerWallCountLeft = boardState.BottomPlayer.WallsToPlace;

				if (boardState.CurrentMover.PlayerType == PlayerType.BottomPlayer)
				{
					if (GameProgress.Count > 0)
						GameProgress[GameProgress.Count - 1] = GameProgress[GameProgress.Count - 1] + $" {boardState.LastMove}";

					StopTimer();
				}
				else
				{
					GameProgress.Add($"{GameProgress.Count+1}. {boardState.LastMove}");
					StartTimer();
				}
			}			
		}

		private void StartTimer()
		{
			TopPlayerRestTime = "60";
			botCountDownTimer.Start();
		}

		private void StopTimer()
		{
			botCountDownTimer.Stop();
			TopPlayerRestTime = "--";
		}

		public IBoardViewModel BoardViewModel { get; }
		public IBoardPlacementViewModel BoardPlacementViewModel { get; }

		public ICommand Start         { get; }
		
		public ICommand ShowAboutHelp { get; }
		public ICommand Capitulate    { get; }
		public ICommand ApplyMove     { get; }
		public ICommand BrowseDll     { get; }

		public ObservableCollection<string> DebugMessages { get; }
		public ObservableCollection<string> GameProgress  { get; }

		public bool IsAutoScrollProgressActive
		{
			get { return isAutoScrollProgressActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollProgressActive, value); }
		}

		public bool IsAutoScrollDebugMsgActive
		{
			get { return isAutoScrollDebugMsgActive; }
			set { PropertyChanged.ChangeAndNotify(this, ref isAutoScrollDebugMsgActive, value); }
		}

		public bool IsDisabledOverlayVisible
		{
			get { return isDisabledOverlayVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isDisabledOverlayVisible, value); }
		}

		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set
			{
				IsDisabledOverlayVisible = value != GameStatus.Active;

				PropertyChanged.ChangeAndNotify(this, ref gameStatus, value);
			}
		}


		public string TopPlayerName
		{
			get { return topPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerName, value); }
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

		public int BottomPlayerWallCountLeft
		{
			get { return bottomPlayerWallCountLeft; }
			private set { PropertyChanged.ChangeAndNotify(this, ref bottomPlayerWallCountLeft, value); }
		}

		public string MoveInput
		{
			get { return moveInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref moveInput, value); }
		}

		public string DllPathInput
		{
			get { return dllPathInput; }
			set { PropertyChanged.ChangeAndNotify(this, ref dllPathInput, value); }
		}		


		private void DoBrowseDll()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "dll|*.dll"
			};

			var result = dialog.ShowDialog();

			if (result.HasValue)
				if (result.Value)
					DllPathInput = dialog.FileName;
		}

		private void DoStart()
		{
			if (GameStatus == GameStatus.Finished)
			{
				gameService.StopGame();

				GameProgress.Clear();
				DebugMessages.Clear();
			}

			if (string.IsNullOrWhiteSpace(DllPathInput))
			{
				MessageBox.Show("bevor das Spiel gestartet werden kann muss eine bot-Dll ausgewählt werden");
				return;
			}

			if (!File.Exists(DllPathInput))
			{
				MessageBox.Show($"die datei {DllPathInput} existiert nicht");
				return;
			}

			lastUsedBotService.SaveLastUsedBot(DllPathInput);			

			GameStatus = GameStatus.Unloaded;

			gameService.CreateGame(DllPathInput, 100);

			((Command)ApplyMove).RaiseCanExecuteChanged();
			((Command)Capitulate).RaiseCanExecuteChanged();
		}
				
		private bool IsMoveApplyable ()
		{
			if (GameStatus != GameStatus.Active)
				return false;

			return gameService.CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer;
		}

		private void DoApplyMove ()
		{
			var move = MoveParser.GetMove(MoveInput, 
										  gameService.CurrentBoardState, 
										  gameService.CurrentBoardState.CurrentMover);

			if (move == null)
			{
				MessageBox.Show("kein gültiger zug");
				return;
			}

			gameService.ReportHumanMove(move);
		}

		private void DoCapitulate ()
		{
			gameService.ReportHumanMove(new Capitulation(gameService.CurrentBoardState,
														 gameService.CurrentBoardState.CurrentMover));
		}

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}