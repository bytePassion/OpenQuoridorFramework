using System;
using System.Collections.Generic;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.ViewModels;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService : IBoardStateProvider
	{
		event Action<ConnectionStatus> ConnectionStatusChanged;		
		event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;
		event Action JoinError;
		event Action<string> JoinSuccessful;
		event Action<string> OpendGameIsStarting;		
		event Action<bool, WinningReason> GameOver;
		
		NetworkGameId CurrentGameId { get; }
		ConnectionStatus CurrentConnectionStatus { get; }

		string PlayerName { get; }
		string GameName   { get; }

		Player TopPlayer      { get; }
		Player BottomPlayer   { get; }
		Player ClientPlayer   { get; }
		Player OpponendPlayer { get; }

		void ConnectToServer(AddressIdentifier serverAddress, string playerName);
		void CreateGame(string gameName, NetworkGameId gameId);
		void JoinGame(NetworkGameId gameId, string gameName);
		void SubmitMove(Move nextMove);
		
		void Disconnect(); 
	}
}
