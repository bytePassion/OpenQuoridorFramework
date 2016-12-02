using System;
using System.Collections.Generic;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.Contracts
{
	public interface IGameRepository
	{
		event Action RepositoryChanged;

		INetworkGame GetGameByPlayer(ClientId clientId);
		INetworkGame GetGameById(NetworkGameId gameId);
		void CreateGame(NetworkGameId gameId, ClientInfo gameInitiator, string gameName);
		void DeleteGame(NetworkGameId gameId);
		IEnumerable<INetworkGame> GetAllGames();
		void ClearRepository();
	}
}