using System;
using System.Collections.Generic;
using System.Text;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class OpenGameListUpdateNotification : NetworkMessageBase.NetworkMessageBase
	{
		public OpenGameListUpdateNotification(ClientId clientId, IDictionary<Guid, string> openGames) 
			: base(NetworkMessageType.OpenGameListUpdateNotification, clientId)
		{
			OpenGames = openGames;
		}

		private IDictionary<Guid, string> OpenGames { get; }

		public override string AsString()
		{
			var sb = new StringBuilder();

			foreach (var gamePair in OpenGames)
			{
				sb.Append(gamePair.Key);
				sb.Append(",");
				sb.Append(gamePair.Value);
				sb.Append(";");
			}

			if (OpenGames.Count > 0)
				sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}

		public static OpenGameListUpdateNotification Parse (ClientId clientId, string s)
		{
			var games = new Dictionary<Guid, string>();

			var pairs = s.Split(';');

			foreach (var pair in pairs)
			{
				var parts = pair.Split(',');

				games.Add(Guid.Parse(parts[0]), parts[1]);
			}

			return new OpenGameListUpdateNotification(clientId, games);			
		}
	}
}