using System;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;

namespace OQF.Net.LanServer.Contracts
{
	public interface INetworkGame
	{
		event Action GameStatusChanged;
		event Action<INetworkGame, BoardState> NewBoardStateAvailable;
		event Action<INetworkGame, ClientInfo, WinningReason> WinnerAvailable;

		bool IsGameActive { get; }
		string GameName { get; }
		ClientInfo GameInitiator { get; }
		ClientInfo Opponend { get; }
		NetworkGameId GameId { get; }
		BoardState CurrentBoardState { get; }

		void StartGame(ClientInfo opponend);
		void ReportMove(ClientInfo sender, Move newMove);
	}
}