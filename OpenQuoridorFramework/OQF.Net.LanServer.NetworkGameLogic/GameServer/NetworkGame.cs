using System;
using OQF.AnalysisAndProgress.Analysis;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.Enum;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class NetworkGame
	{
		private BoardState currentBoardState;
		public event Action<NetworkGame, BoardState> NewBoardStateAvailable;
		public event Action<NetworkGame, ClientInfo, WinningReason> WinnerAvailable;

		public NetworkGame(string gameName, ClientInfo gameInitiator)
		{
			GameName = gameName;
			GameInitiator = gameInitiator;
			IsGameActive = false;
		}

		public bool IsGameActive { get; private set; }
		public string GameName { get; }
		public ClientInfo GameInitiator { get; }
		public ClientInfo Opponend { get; private set; }

		public BoardState CurrentBoardState
		{
			get { return currentBoardState; }
			private set
			{
				currentBoardState = value;
				NewBoardStateAvailable?.Invoke(this, currentBoardState);
			}
		}

		public void StartGame(ClientInfo opponend)
		{
			Opponend = opponend;

			GameInitiator.CreatePlayer(PlayerType.BottomPlayer);
			Opponend.CreatePlayer(PlayerType.TopPlayer);

			CurrentBoardState = BoardStateTransition.CreateInitialBoadState(Opponend.Player, GameInitiator.Player);

			IsGameActive = true;
		}		

		public void ReportMove(ClientInfo sender, Move newMove)
		{
			if (!IsGameActive)
				return;

			if (CurrentBoardState.CurrentMover == sender.Player)
			{
				CurrentBoardState = CurrentBoardState.ApplyMove(newMove);

				if (!GameAnalysis.IsMoveLegal(CurrentBoardState, newMove))
				{
					WinnerAvailable?.Invoke(this, 
											CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer ? Opponend : GameInitiator,
											WinningReason.InvalidMove);
					IsGameActive = false;
				}

				CurrentBoardState = CurrentBoardState.ApplyMove(newMove);

				if (newMove is Capitulation)
				{
					WinnerAvailable?.Invoke(this,
											CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer ? GameInitiator : Opponend,
											WinningReason.Capitulation);
					IsGameActive = false;
				}

				var winner = GameAnalysis.CheckWinningCondition(CurrentBoardState);

				if (winner != null)
				{
					WinnerAvailable?.Invoke(this, 
											winner == GameInitiator.Player ? GameInitiator : Opponend, 
											WinningReason.RegularQuoridorWin);
					IsGameActive = false;
				}
			}
			else
			{
				WinnerAvailable?.Invoke(this, 
										CurrentBoardState.CurrentMover.PlayerType == PlayerType.BottomPlayer ? GameInitiator : Opponend, 
										WinningReason.InvalidMove);
				IsGameActive = false;
			}
		}
	}
}