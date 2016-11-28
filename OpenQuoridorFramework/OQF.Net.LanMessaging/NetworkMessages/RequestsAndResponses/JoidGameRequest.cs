using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class JoinGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public JoinGameRequest (ClientId clientId, NetworkGameId gameId) 
			: base(NetworkMessageType.JoinGameRequest, clientId)
		{
			GameId = gameId;
		}

		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static JoinGameRequest Parse (ClientId clientId, string s)
		{
			return new JoinGameRequest(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}