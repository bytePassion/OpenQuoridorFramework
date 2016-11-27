using System.Collections.Generic;
using System.Linq;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class GameRepository : IGameRepository
	{
		private readonly IDictionary<NetworkGameId, NetworkGame> games;

		public GameRepository()
		{
			games = new Dictionary<NetworkGameId, NetworkGame>();
		}

		public NetworkGame GetGameByPlayer(ClientId clientId)
		{
			return games.FirstOrDefault(game => game.Value.GameInitiator.ClientId == clientId || game.Value.Opponend?.ClientId == clientId).Value;
		}

		public NetworkGame GetGameById(NetworkGameId gameId)
		{
			throw new System.NotImplementedException();
		}

		public void CreateGame(NetworkGameId gameId, ClientInfo gameInitiator, string gameName)
		{
			games.Add(gameId, new NetworkGame(gameName, gameInitiator));
		}

		public void DeleteGame(NetworkGameId gameId)
		{
			games.Remove(gameId);
		}
		
		public void ClearRepository()
		{
			games.Clear();
		}
	}
}