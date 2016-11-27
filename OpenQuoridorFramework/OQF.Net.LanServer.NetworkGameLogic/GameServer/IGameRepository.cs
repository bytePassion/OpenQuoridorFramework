using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public interface IGameRepository
	{
		NetworkGame GetGameByPlayer(ClientId clientId);
		void CreateGame(ClientInfo gameInitiator, string gameName);
		void DeleteGame(NetworkGame game);
		void ClearRepository();
	}
}