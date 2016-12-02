using System;
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

		public event Action RepositoryChanged;

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
			var newGame = new NetworkGame(gameName, gameInitiator, gameId);

			newGame.GameStatusChanged += OnGameStatusChanged;

			games.Add(gameId, newGame);
			RepositoryChanged?.Invoke();
		}

		private void OnGameStatusChanged()
		{
			RepositoryChanged?.Invoke();
		}

		public void DeleteGame(NetworkGameId gameId)
		{
			var gameToDelete = games[gameId];

			gameToDelete.GameStatusChanged -= OnGameStatusChanged;

			games.Remove(gameId);
			RepositoryChanged?.Invoke();
		}

		public IEnumerable<NetworkGame> GetAllGames()
		{
			return games.Values;
		}

		public void ClearRepository()
		{
			foreach (var networkGame in games.Values)
			{
				networkGame.GameStatusChanged -= OnGameStatusChanged;
			}

			games.Clear();
		}
	}
}