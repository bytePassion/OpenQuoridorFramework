using System;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService
	{
		event Action GotConnected;
		
		void ConnectToServer(AddressIdentifier serverAddress, string playerName);
		void CreateGame(string gameName, NetworkGameId gameId);
		void Dissconnect(); 
	}
}
