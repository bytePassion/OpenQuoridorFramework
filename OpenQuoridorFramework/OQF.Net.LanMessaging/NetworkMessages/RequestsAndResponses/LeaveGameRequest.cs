using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class LeaveGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public LeaveGameRequest(ClientId clientId, NetworkGameId gameId) 
			: base(NetworkMessageType.LeaveGameRequest, clientId)
		{
			GameId = gameId;
		}

		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static LeaveGameRequest Parse(ClientId clientId, string s)
		{
			return new LeaveGameRequest(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}