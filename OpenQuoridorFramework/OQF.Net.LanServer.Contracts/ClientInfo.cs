using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanServer.Contracts
{
	public class ClientInfo
	{
		public ClientInfo(ClientId clientId, string playerName)
		{
			ClientId = clientId;
			PlayerName = playerName;
		}

		public ClientId ClientId { get; }
		public string PlayerName { get; }

		public Player Player { get; private set; }

		public void CreatePlayer(PlayerType playerType)
		{
			Player = new Player(playerType, PlayerName);
		}
	}
}