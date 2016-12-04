using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class CreateGameResponse : NetworkMessageBase.NetworkMessageBase
	{
		public CreateGameResponse(ClientId clientId, NetworkGameId gameId) 
			: base(NetworkMessageType.CreateGameResponse, clientId)
		{			
			GameId = gameId;
		}
		
		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return GameId.ToString();
		}

		public static CreateGameResponse Parse (ClientId clientId, string s)
		{
			return new CreateGameResponse(clientId, new NetworkGameId(Guid.Parse(s)));
		}
	}
}