using System.Collections.Generic;
using System.Linq;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class GameRepository : IGameRepository
	{
		private readonly IList<NetworkGame> games;

		public GameRepository()
		{
			games = new List<NetworkGame>();
		}

		public NetworkGame GetGameByPlayer(ClientId clientId)
		{
			return games.FirstOrDefault(game => game.GameInitiator.ClientId == clientId || game.Opponend?.ClientId == clientId);
		}

		public void CreateGame(ClientInfo gameInitiator, string gameName)
		{
			games.Add(new NetworkGame(gameName, gameInitiator));
		}

		public void DeleteGame(NetworkGame game)
		{
			games.Remove(game);
		}

		public void ClearRepository()
		{
			games.Clear();
		}
	}
}