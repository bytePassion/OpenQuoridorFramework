using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public interface IGameRepository
	{
		NetworkGame GetGameByPlayer(ClientId clientId);
		NetworkGame GetGameById(NetworkGameId gameId);
		void CreateGame(NetworkGameId gameId, ClientInfo gameInitiator, string gameName);
		void DeleteGame(NetworkGameId gameId);
		void ClearRepository();
	}
}