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
			if (games.ContainsKey(gameId))
				return games[gameId];
			else			
				return null;
			
		}

		public void CreateGame(NetworkGameId gameId, ClientInfo gameInitiator, string gameName)
		{
			games.Add(gameId, new NetworkGame(gameName, gameInitiator, gameId));
		}

		public void DeleteGame(NetworkGameId gameId)
		{
			games.Remove(gameId);
		}

		public IEnumerable<NetworkGame> GetAllGames()
		{
			return games.Values;
		}

		public void ClearRepository()
		{
			games.Clear();
		}
	}
}