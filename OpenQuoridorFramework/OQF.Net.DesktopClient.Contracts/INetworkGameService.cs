using System;
using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService
	{
		event Action GotConnected;

		void ConnectToServer(AddressIdentifier serverAddress, string playerName);
		void CreateGame(string gameName, Guid gameId);
		void Dissconnect(); 
	}
}
