using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class CancelCreatedGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public CancelCreatedGameRequest(ClientId clientId, NetworkGameId gameId)
			: base(NetworkMessageType.CancelCreatedGameRequest, clientId)
		{
			GameId = gameId;
		}

		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static CancelCreatedGameRequest Parse(ClientId clientId, string s)
		{
			return new CancelCreatedGameRequest(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}