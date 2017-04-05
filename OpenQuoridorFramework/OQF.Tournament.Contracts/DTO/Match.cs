using System;
using OQF.AnalysisAndProgress.Analysis;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.Enum;

namespace OQF.Tournament.Contracts.DTO
{
	public class Match
    {
        private BoardState currentBoardState;

        
        public BoardState CurrentBoardState
        {
            get { return currentBoardState; }
            private set
            {
                currentBoardState = value;
                NewBoardStateAvailable?.Invoke(this, currentBoardState);
            }
        }

        public Player TopPlayer { get; set; }

        public Player BottomPlayer { get;  set; }

        public string BottomPath { get;  }
        public string TopPath { get;  }
        public GameConstraints Constraints { get; }

        public event Action<Match, BoardState> NewBoardStateAvailable;
        public event Action<Match, string, WinningReason> WinnerAvailable;

        public Match(string bPath, string tPath, GameConstraints constraints)
        {
            BottomPath = bPath;
            TopPath = tPath;
            Constraints = constraints;
        }

        public void StartMatch()
        {
            CurrentBoardState = BoardStateTransition.CreateInitialBoadState(TopPlayer, BottomPlayer);
        }

        public void ReportMove(Move nextMove)
        {
            if (!GameAnalysis.IsMoveLegal(CurrentBoardState, nextMove))
            {
                WinnerAvailable?.Invoke(this,
                    CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer
                        ? TopPlayer.Name
                        : BottomPlayer.Name,
                    WinningReason.InvalidMove);
            }

            CurrentBoardState = CurrentBoardState.ApplyMove(nextMove);

            if (nextMove is Capitulation)
            {
                WinnerAvailable?.Invoke(this,
                    CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer
                        ? BottomPlayer.Name
                        : TopPlayer.Name,
                    WinningReason.Capitulation);
            }

            var winner = GameAnalysis.CheckWinningCondition(CurrentBoardState);

            if (winner != null)
            {
                WinnerAvailable?.Invoke(this,
                    winner == BottomPlayer ? BottomPlayer.Name : TopPlayer.Name,
                    WinningReason.RegularQuoridorWin);
            }
        }

    }
}