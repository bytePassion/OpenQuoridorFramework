using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class LeaveGame : NetworkMessageBase.NetworkMessageBase
	{
		public LeaveGame(ClientId clientId, NetworkGameId gameId) 
			: base(NetworkMessageType.LeaveGame, clientId)
		{
			GameId = gameId;
		}

		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static LeaveGame Parse(ClientId clientId, string s)
		{
			return new LeaveGame(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}