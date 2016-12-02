using System;
using OQF.AnalysisAndProgress.Analysis;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Net.LanMessaging.Types;
using OQF.Net.LanServer.Contracts;
using OQF.Utils.BoardStateUtils;
using OQF.Utils.Enum;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class NetworkGame : INetworkGame
	{
		private BoardState currentBoardState;
		private bool isGameActive;
		public event Action GameStatusChanged;
		public event Action<INetworkGame, BoardState> NewBoardStateAvailable;
		public event Action<INetworkGame, ClientInfo, WinningReason> WinnerAvailable;

		public NetworkGame(string gameName, ClientInfo gameInitiator, NetworkGameId gameId)
		{
			GameName = gameName;
			GameInitiator = gameInitiator;
			GameId = gameId;
			IsGameActive = false;
		}

		public bool IsGameActive
		{
			get { return isGameActive; }
			private set
			{
				isGameActive = value;
				GameStatusChanged?.Invoke();
			}
		}

		public string GameName { get; }
		public ClientInfo GameInitiator { get; }
		public ClientInfo Opponend { get; private set; }
		public NetworkGameId GameId { get; }

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