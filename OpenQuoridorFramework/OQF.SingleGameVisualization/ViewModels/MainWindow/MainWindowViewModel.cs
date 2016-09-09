using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FrameworkExtensionLib;
using Microsoft.Win32;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;
using OQF.SingleGameVisualization.Services;
using OQF.SingleGameVisualization.ViewModels.AboutHelpWindow;
using OQF.SingleGameVisualization.ViewModels.Board;
using OQF.SingleGameVisualization.ViewModels.BoardPlacement;
using OQF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using OQF.Utils;
using UtilsLib;
using WpfLib.Commands;
using WpfLib.Commands.Updater;
using WpfLib.ViewModelBase;

namespace OQF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly Timer botCountDownTimer;
		private DateTime startTime;

		private readonly IGameService gameService;
		private readonly ILastUsedBotService lastUsedBotService;

		private string dllPathInput;		
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
			Capitulate = new Command(DoCapitulate,
									 IsMoveApplyable,
									 new PropertyChangedCommandUpdater(this, nameof(GameStatus)));
			ShowAboutHelp = new Command(DoShowAboutHelp);

			GameStatus = GameStatus.Unloaded;

			DllPathInput = lastUsedBotService.GetLastUsedBot();

			botCountDownTimer = new Timer(BotCountDownTimerOnTick, null,Timeout.Infinite, Timeout.Infinite);			
		}		

		private void BotCountDownTimerOnTick(object sender)
		{
			var timeDiff = DateTime.Now - startTime;

			Application.Current?.Dispatcher.Invoke(() =>
			{
				TopPlayerRestTime = GeometryLibUtils.DoubleFormat(60.0 - timeDiff.TotalSeconds, 2);
			});			
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
			StopTimer();

			var winOrLooseMsg = player.PlayerType == PlayerType.TopPlayer
						? $"Sry ... the bot '{player.Name}' beated you\nReason: {winningReason}"
						: $"congratulations! you beated the bot\nReason: {winningReason}";

			var completeMsg = winOrLooseMsg + "\n\nDo you want to Save the Game?";

			var dialogResult = MessageBox.Show(Application.Current.MainWindow, 
											   completeMsg, 
											   "Game over", 
											   MessageBoxButton.YesNo);

			if (dialogResult == MessageBoxResult.Yes)
			{
				var dialog = new SaveFileDialog()
				{
					Filter = "textFiles |*.txt",
					AddExtension = true,
					CheckFileExists = false,
					OverwritePrompt = true,
					ValidateNames = true,
					CheckPathExists = true,
					CreatePrompt = false,
					Title = "Save Game Progress of currently ended game"
				};

				var result = dialog.ShowDialog();

				if (result.HasValue)
				{
					if (result.Value)
					{
						var fileText = CreateProgressText.FromBoardState(gameService.CurrentBoardState)
														 .AndAppendWinnerAndReason(player, winningReason);

						File.WriteAllText(dialog.FileName, fileText);
					}
				}
			}

			GameStatus = GameStatus.Finished;			
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{			
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
			startTime = DateTime.Now;
			TopPlayerRestTime = "60";
			botCountDownTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
		}

		private void StopTimer()
		{
			startTime = default(DateTime);
			botCountDownTimer.Change(Timeout.Infinite, Timeout.Infinite);
			TopPlayerRestTime = "--";
		}

		public IBoardViewModel BoardViewModel { get; }
		public IBoardPlacementViewModel BoardPlacementViewModel { get; }

		public ICommand Start         { get; }		
		public ICommand ShowAboutHelp { get; }
		public ICommand Capitulate    { get; }		
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

			GameStatus = GameStatus.Unloaded;

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

			Assembly dllToLoad;

			try
			{
				dllToLoad = Assembly.LoadFile(DllPathInput);
			}
			catch
			{
				MessageBox.Show($"die datei {DllPathInput} kann nicht als Assembly geladen werden");
				return;
			}

			var uninitializedBot = BotLoader.LoadBot(dllToLoad);

			if (uninitializedBot == null)
			{
				MessageBox.Show($"die Assemply {dllToLoad.FullName} kann nicht als IQuoridorBot instantiiert werden");
				return;
			}

			lastUsedBotService.SaveLastUsedBot(DllPathInput);									
			gameService.CreateGame(uninitializedBot, new GameConstraints(TimeSpan.FromSeconds(60), 100));
			
			((Command)Capitulate).RaiseCanExecuteChanged();
		}
				
		private bool IsMoveApplyable ()
		{
			if (GameStatus != GameStatus.Active)
				return false;

			return gameService.CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer;
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