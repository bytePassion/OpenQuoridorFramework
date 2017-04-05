using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using OQF.AnalysisAndProgress.Analysis;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.ViewModels;
using OQF.Tournament.Contracts;
using OQF.Tournament.Contracts.DTO;
using OQF.Tournament.Contracts.Logger;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.Enum;

namespace OQF.Tournament.Services.Game
{
	public class TournamentGame : DisposingObject, IBoardStateProvider
	{
		public event Action<TournamentParticipant, QProgress, WinningReason> WinnerAvailable;
		
		private GameConstraints constraints;
		private Guid bottomProcessId;
		private readonly IProcessService processService;
		private readonly IDataLogger dataLogger;
		private Guid topProcessId;
		private volatile bool gameFinished;

		private TournamentParticipant bottomParticipant;
		private TournamentParticipant topParticipant;

		private Timer thinkingTimer;
		private volatile bool hasTimoutOccured;
		private BoardState _currentBoardState;

		private readonly Stopwatch moveStopwatch;

		public TournamentGame(IProcessService processService, IDataLogger dataLogger)
		{
			this.processService = processService;
			this.dataLogger = dataLogger;

			moveStopwatch = new Stopwatch();
		}		

		private void StartTimer (TournamentParticipant opponend, BoardState currentBoardState)
		{
			hasTimoutOccured = false;
			thinkingTimer = new Timer(TimerTick,
				new object[] { opponend, currentBoardState, DateTime.Now },
				TimeSpan.FromSeconds(1),
				TimeSpan.FromSeconds(1));
		}

		private bool StopTimerAndCheckIfTimeWasOk ()
		{
			thinkingTimer?.Dispose();
			thinkingTimer = null;

			return !gameFinished && !hasTimoutOccured;
		}

		private void TimerTick (object playerInfo)
		{
			var opponend   = (playerInfo as object[])[0] as TournamentParticipant;
			var boardState = (playerInfo as object[])[1] as BoardState;
			var startingTime =(DateTime)(playerInfo as object[])[2];

			var now = DateTime.Now;

			if (now - startingTime > constraints.MaximalComputingTimePerMove + TimeSpan.FromSeconds(5))
			{
				thinkingTimer.Dispose();
				thinkingTimer = null;

				hasTimoutOccured = true;

				if (!gameFinished)
				{
					moveStopwatch.Reset();

					ReportWinAndCleanUp(opponend == bottomParticipant
											? PlayerType.BottomPlayer 
											: PlayerType.TopPlayer,
										boardState == null
											? null
											: CreateQProgress.FromBoardState(boardState),
										WinningReason.ExceedanceOfThoughtTime);
				}
			}			
		}

		public void StartGame (GameConstraints gameConstraints, TournamentParticipant bottom, TournamentParticipant top)
		{
			gameFinished = false;
			dataLogger.ReportLog($"Next match started: {bottom.Name} vs. {top.Name}", LogLevel.OnlyGameResults);
			
			constraints = gameConstraints;
			bottomParticipant = bottom;
			topParticipant = top;

			bottomProcessId = processService.CreateBotProcess();
			topProcessId = processService.CreateBotProcess();

			LoadBottomPlayer(); // HERE starts the game-loop			
		}

		private void LoadBottomPlayer ()
		{
			if (gameFinished) return;
			dataLogger.ReportLog("load bottom player", LogLevel.All);			
			StartTimer(topParticipant, null);

			processService.LoadBot(bottomProcessId, bottomParticipant.DllPath, PlayerType.BottomPlayer, () =>
			{
				if (StopTimerAndCheckIfTimeWasOk())
				{
					LoadTopPlayer();
				}
			});
		}

		private void LoadTopPlayer ()
		{
			if (gameFinished) return;
			dataLogger.ReportLog("load top player", LogLevel.All);			
			StartTimer(bottomParticipant, null);

			processService.LoadBot(topProcessId, topParticipant.DllPath, PlayerType.TopPlayer, () =>
			{
				if (StopTimerAndCheckIfTimeWasOk())
				{
					InitBottomPlayer();
				}
			});
		}

		private void InitBottomPlayer ()
		{
			if (gameFinished) return;
			dataLogger.ReportLog("init bottom player", LogLevel.All);			
			StartTimer(topParticipant, null);

			processService.InitBot(bottomProcessId, constraints, PlayerType.BottomPlayer, () =>
			{
				if (StopTimerAndCheckIfTimeWasOk())
				{
					InitTopPlayer();
				}
			});
		}

		private void InitTopPlayer ()
		{
			if (gameFinished) return;
			dataLogger.ReportLog("init top player", LogLevel.All);			
			StartTimer(bottomParticipant, null);

			processService.InitBot(topProcessId, constraints, PlayerType.TopPlayer, () =>
			{
				if (StopTimerAndCheckIfTimeWasOk())
				{					
					StartMoves();
				}
			});
		}

		private void NextMoveFromBottomPlayer (BoardState currentBoardState)
		{
			if (gameFinished) return;
			dataLogger.ReportLog("bottom player move", LogLevel.All);
			StartTimer(topParticipant, currentBoardState);

			moveStopwatch.Reset();
			moveStopwatch.Start();
			
			processService.DoMove(
				bottomProcessId,
				CreateQProgress.FromBoardState(currentBoardState),
				nextMove =>
				{
					moveStopwatch.Stop();
					dataLogger.ReportMoveTime(bottomParticipant, moveStopwatch.Elapsed);

					if (gameFinished) return;
					
					if (StopTimerAndCheckIfTimeWasOk())
					{
						var nextBoardState = ProcessMove(currentBoardState, PlayerType.BottomPlayer, nextMove);
						if (gameFinished) return;						
						NextMoveFromTopPlayer(nextBoardState);
					}
				}
			);
		}

		private void NextMoveFromTopPlayer (BoardState currentBoardState)
		{
			if (gameFinished) return;
			dataLogger.ReportLog("top player move", LogLevel.All);
			StartTimer(bottomParticipant, currentBoardState);

			moveStopwatch.Reset();
			moveStopwatch.Start();

			processService.DoMove(
				topProcessId, 
				CreateQProgress.FromBoardState(currentBoardState), 
				nextMove =>
				{
					moveStopwatch.Stop();
					dataLogger.ReportMoveTime(topParticipant, moveStopwatch.Elapsed);

					if (gameFinished) return;
				
					if (StopTimerAndCheckIfTimeWasOk())
					{
						var nextBoardState = ProcessMove(currentBoardState, PlayerType.TopPlayer, nextMove);
						if (gameFinished) return;					
						NextMoveFromBottomPlayer(nextBoardState);
					}
				}
			);
		}

		private void ReportWinAndCleanUp (PlayerType winnerType, QProgress progress, WinningReason winningReason)
		{
			CleanUpAfterGameEnds();	

			if (progress != null)
			{
				var movesBottomPlayer = new List<Move>();
				var movesTopPlayer    = new List<Move>();

				var allMoves = progress.Moves.ToList();

				for (int i = 0; i < allMoves.Count; i++)
				{
					if (i%2 == 0)
						movesBottomPlayer.Add(allMoves[i]);
					else
						movesTopPlayer.Add(allMoves[i]);
				}

				var moveCountBottomPlayer = movesBottomPlayer.Count;
				var moveCountTopPlayer    = movesTopPlayer.Count;

				var wallCountBottomPlayer = movesBottomPlayer.Count(move => move is WallMove);
				var wallCountTopPlayer    = movesTopPlayer.Count(move => move is WallMove);

				dataLogger.ReportWallCount(bottomParticipant, wallCountBottomPlayer, winnerType == PlayerType.BottomPlayer);
				dataLogger.ReportWallCount(topParticipant,    wallCountTopPlayer,    winnerType == PlayerType.TopPlayer);

				dataLogger.ReportMoveCount(bottomParticipant, moveCountBottomPlayer, winnerType == PlayerType.BottomPlayer);
				dataLogger.ReportMoveCount(topParticipant,    moveCountTopPlayer,    winnerType == PlayerType.TopPlayer);
			} 

			dataLogger.ReportResult(bottomParticipant, winnerType == PlayerType.BottomPlayer);		
			dataLogger.ReportResult(topParticipant,    winnerType == PlayerType.TopPlayer);
			
			WinnerAvailable?.Invoke(winnerType == PlayerType.BottomPlayer ? bottomParticipant : topParticipant, progress, winningReason);		
		}

		private void CleanUpAfterGameEnds()
		{
			moveStopwatch.Reset();
			gameFinished = true;
			processService.KillBotProcess(bottomProcessId);
			processService.KillBotProcess(topProcessId);
		}

		private void StartMoves ()
		{
			var currentBoardState = BoardStateTransition.CreateInitialBoadState(
				new Player(PlayerType.TopPlayer, string.Empty),
				new Player(PlayerType.BottomPlayer, string.Empty));

			NextMoveFromBottomPlayer(currentBoardState);
		}

		private PlayerType GetOther(PlayerType pt)
		{
			if (pt == PlayerType.BottomPlayer)
				return PlayerType.TopPlayer;
			else			
				return PlayerType.BottomPlayer;			
		}

		private BoardState ProcessMove (BoardState currentBoardState, PlayerType player, Move newMove)
		{
			if (gameFinished)
				return null;

			dataLogger.ReportLog($"Move[{newMove} by {(player == PlayerType.TopPlayer ? topParticipant.Name : bottomParticipant.Name)}]", LogLevel.GameActivities);
			
			if (currentBoardState.CurrentMover.PlayerType == player)
			{
				if (!GameAnalysis.IsMoveLegal(currentBoardState, newMove))
				{
					ReportWinAndCleanUp(GetOther(currentBoardState.CurrentMover.PlayerType), 
										CreateQProgress.FromBoardState(currentBoardState), 
										WinningReason.InvalidMove);					
					return null;
				}

				currentBoardState = currentBoardState.ApplyMove(newMove);

				CurrentBoardState = currentBoardState;				

				if (newMove is Capitulation)
				{
					ReportWinAndCleanUp(GetOther(currentBoardState.CurrentMover.PlayerType),
										CreateQProgress.FromBoardState(currentBoardState),
										WinningReason.Capitulation);
					return null;
				}

				var matchWinner = GameAnalysis.CheckWinningCondition(currentBoardState);

				if (matchWinner != null)
				{
					ReportWinAndCleanUp(matchWinner.PlayerType,
										CreateQProgress.FromBoardState(currentBoardState),
										WinningReason.RegularQuoridorWin);
					return null;
				}
			}
			else
			{
				ReportWinAndCleanUp(currentBoardState.CurrentMover.PlayerType,
									CreateQProgress.FromBoardState(currentBoardState),
									WinningReason.InvalidMove);
				return null;
			}

			return currentBoardState;
		}


		protected override void CleanUp()
		{
			if (!gameFinished)
				CleanUpAfterGameEnds();			
		}

		public event Action<BoardState> NewBoardStateAvailable;

		public BoardState CurrentBoardState
		{
			get { return _currentBoardState; }
			private set
			{
				_currentBoardState = value;
				NewBoardStateAvailable?.Invoke(CurrentBoardState);
			}
		}
	}
}