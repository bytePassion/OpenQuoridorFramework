namespace OQF.Net.LanMessaging.Types
{
	public class NetworkGameInfo
	{
		public NetworkGameInfo(NetworkGameId gameId, string initiatorName, string gameName)
		{
			GameId = gameId;
			InitiatorName = initiatorName;
			GameName = gameName;
		}

		public NetworkGameId GameId { get; }
		public string InitiatorName { get; }
		public string GameName { get; }
	}
}
