using System;
using System.Collections.Generic;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService
	{
		event Action GotConnected;
		event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;

		void ConnectToServer(AddressIdentifier serverAddress, string playerName);
		void CreateGame(string gameName, NetworkGameId gameId);
		void Dissconnect(); 
	}
}
