using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class CreateGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public CreateGameRequest(ClientId clientId, string gameName, Guid gameId) 
			: base(NetworkMessageType.CreateGameRequest, clientId)
		{
			GameName = gameName;
			GameId = gameId;
		}

		public string GameName { get; }
		public Guid GameId { get; }

		public override string AsString()
		{
			return $"{GameName};{GameId}";
		}

		public static CreateGameRequest Parse(ClientId clientId, string s)
		{
			var parts = s.Split(';');

			return new CreateGameRequest(clientId, 
										 parts[0], 
										 Guid.Parse(parts[1]));
		}
	}
}