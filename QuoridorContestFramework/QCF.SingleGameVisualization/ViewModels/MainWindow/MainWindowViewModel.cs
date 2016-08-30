using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.SingleGameVisualization.Services;
using QCF.SingleGameVisualization.ViewModels.Board;
using QCF.SingleGameVisualization.ViewModels.MainWindow.Helper;
using QCF.Tools.FrameworkExtensions;
using QCF.Tools.WpfTools.Commands;
using QCF.Tools.WpfTools.Commands.Updater;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		private readonly IGameService gameService;
		private string dllPathInput;
		private string moveInput;
		private int bottomPlayerWallCountLeft;
		private int topPlayerWallCountLeft;		
		private string topPlayerName;
		private GameStatus gameStatus;		

		public MainWindowViewModel (IBoardViewModel boardViewModel, IGameService gameService)
		{
			this.gameService = gameService;
			BoardViewModel = boardViewModel;
			DebugMessages  = new ObservableCollection<string>();
			GameProgress   = new ObservableCollection<string>();			
			
			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameService.WinnerAvailable        += OnWinnerAvailable;
			gameService.NewDebugMsgAvailable   += OnNewDebugMsgAvailable; 


			BrowseDll = new Command(DoBrowseDll);
			Start = new Command(DoStart,
								() => GameStatus == GameStatus.Unloaded,
								new PropertyChangedCommandUpdater(this, nameof(GameStatus)));			
			ApplyMove = new Command(DoApplyMove,
									IsMoveApplyable,
									new PropertyChangedCommandUpdater(this, nameof(GameStatus)));

			GameStatus = GameStatus.Unloaded;			
		}		

		private void OnNewDebugMsgAvailable(string s)
		{
			DebugMessages.Add(s);
		}

		private void OnWinnerAvailable(Player player)
		{
			MessageBox.Show(
				player.PlayerType == PlayerType.TopPlayer
						? $@"Sry ... the bot '{player.Name}' beated you"
						: "congratulations! you beated the bot"
			);

			GameStatus = GameStatus.Finished;
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			((Command)ApplyMove).RaiseCanExecuteChanged();

			if (boardState == null)
			{
				GameStatus = GameStatus.Unloaded;

				TopPlayerName = string.Empty;
				TopPlayerWallCountLeft = 10;
				BottomPlayerWallCountLeft = 10;
			}
			else
			{
				GameStatus = GameStatus.Active;

				TopPlayerName = boardState.TopPlayer.Player.Name;

				TopPlayerWallCountLeft = boardState.TopPlayer.WallsToPlace;
				BottomPlayerWallCountLeft = boardState.BottomPlayer.WallsToPlace;
			}			
		}

		public IBoardViewModel BoardViewModel { get; }

		public ICommand Start     { get; }
		public ICommand Restart   { get; }
		public ICommand Stop      { get; }
		public ICommand AboutHelp { get; }
		public ICommand ApplyMove { get; }
		public ICommand BrowseDll { get; }

		public ObservableCollection<string> DebugMessages { get; }
		public ObservableCollection<string> GameProgress  { get; }

		public GameStatus GameStatus
		{
			get { return gameStatus; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gameStatus, value); }
		}


		public string TopPlayerName
		{
			get { return topPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref topPlayerName, value); }
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

			gameService.CreateGame(DllPathInput);

			((Command)Start).RaiseCanExecuteChanged();
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

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}