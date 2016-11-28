﻿using System;
using System.Collections.Generic;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.Moves;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService
	{
		event Action GotConnected;
		event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;
		event Action JoinError;
		event Action<string> JoinSuccessful;
		event Action<QProgress> NewProgressAvailable;
		event Action<bool, WinningReason> GameOver;

		QProgress CurrentProgress { get; }
		NetworkGameId CurrentGameId { get; }

		void ConnectToServer(AddressIdentifier serverAddress, string playerName);
		void CreateGame(string gameName, NetworkGameId gameId);
		void JoinGame(NetworkGameId gameId);
		void SubmitMove(Move nextMove);

		void Dissconnect(); 
	}
}
