using System.Collections.Generic;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public interface IClientRepository
	{
		ClientInfo GetClientById(ClientId clientId);
		bool IsClientIdRegistered(ClientId clientId);
		void AddClient(ClientId clientId, string playerName);
		void RemoveClient(ClientId clientId);
		IEnumerable<ClientInfo> GetAllClients();
		void ClearRepository();
	}
}