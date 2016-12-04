using System;
using System.Collections.Generic;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.Contracts
{
	public interface IClientRepository
	{
		event Action RepositoryChanged;

		ClientInfo GetClientById(ClientId clientId);
		bool IsClientIdRegistered(ClientId clientId);
		void AddClient(ClientId clientId, string playerName);
		void RemoveClient(ClientId clientId);
		IEnumerable<ClientInfo> GetAllClients();
		void ClearRepository();
	}
}