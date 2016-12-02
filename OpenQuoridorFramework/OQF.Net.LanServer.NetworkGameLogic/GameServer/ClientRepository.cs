using System.Collections.Generic;
using OQF.Net.LanMessaging.Types;
using OQF.Net.LanServer.Contracts;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class ClientRepository : IClientRepository
	{
		private readonly IDictionary<ClientId, ClientInfo> clients;

		public ClientRepository()
		{
			clients = new Dictionary<ClientId, ClientInfo>();
		}

		public ClientInfo GetClientById(ClientId clientId)
		{
			if (IsClientIdRegistered(clientId))
				return clients[clientId];
			else
				return null;
		}

		public bool IsClientIdRegistered(ClientId clientId)
		{
			return clients.ContainsKey(clientId);
		}

		public void AddClient(ClientId clientId, string playerName)
		{
			if (!IsClientIdRegistered(clientId))
				clients.Add(clientId, new ClientInfo(clientId, playerName));
		}

		public void RemoveClient(ClientId clientId)
		{
			if (IsClientIdRegistered(clientId))
				clients.Remove(clientId);
		}

		public IEnumerable<ClientInfo> GetAllClients()
		{
			return clients.Values;
		}

		public void ClearRepository()
		{
			clients.Clear();
		}
	}
}
