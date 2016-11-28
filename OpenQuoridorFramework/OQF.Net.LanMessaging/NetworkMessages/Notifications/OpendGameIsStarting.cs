using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class OpendGameIsStarting : NetworkMessageBase.NetworkMessageBase
	{
		public OpendGameIsStarting (ClientId clientId, NetworkGameId gameId, string opponendPlayerName) 
			: base(NetworkMessageType.OpendGameIsStarting, clientId)
		{
			GameId = gameId;
			OpponendPlayerName = opponendPlayerName;
		}

		public NetworkGameId GameId { get; }		
		public string OpponendPlayerName { get; }

		public override string AsString ()
		{
			return $"{GameId};{OpponendPlayerName}";
		}

		public static OpendGameIsStarting Parse(ClientId clientId, string s)
		{
			var parts = s.Split(';'); 

			var gameId = new NetworkGameId(Guid.Parse(parts[0]));
			var oppenendPlayerName = parts[1];

			return new OpendGameIsStarting(clientId, gameId, oppenendPlayerName);
		}
	}
}