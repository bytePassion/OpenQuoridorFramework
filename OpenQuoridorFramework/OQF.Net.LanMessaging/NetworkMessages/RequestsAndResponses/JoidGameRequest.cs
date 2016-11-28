using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class JoidGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public JoidGameRequest(ClientId clientId, NetworkGameId gameId) 
			: base(NetworkMessageType.JoinGameRequest, clientId)
		{
			GameId = gameId;
		}

		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static JoidGameRequest Parse(ClientId clientId, string s)
		{
			return new JoidGameRequest(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}