using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class JoinGameResponse : NetworkMessageBase.NetworkMessageBase
	{
		public JoinGameResponse(ClientId clientId, NetworkGameId gameId, bool joinSuccessful, string opponendPlayerName) 
			: base(NetworkMessageType.JoinGameResponse, clientId)
		{
			GameId = gameId;
			JoinSuccessful = joinSuccessful;
			OpponendPlayerName = opponendPlayerName;			
		}

		public NetworkGameId GameId { get; }
		public bool JoinSuccessful { get; }
		public string OpponendPlayerName { get; }				

		public override string AsString()
		{
			return $"{GameId};{JoinSuccessful};{OpponendPlayerName};";
		}

		public static JoinGameResponse Parse(ClientId clientId, string s)
		{
			var parts = s.Split(';');

			var gameId = new NetworkGameId(Guid.Parse(parts[0]));
			var joinSuccessful = bool.Parse(parts[1]);
			var opponendPlayerName = parts[2];			

			return new JoinGameResponse(clientId, gameId, joinSuccessful, opponendPlayerName);
		}
	}
}