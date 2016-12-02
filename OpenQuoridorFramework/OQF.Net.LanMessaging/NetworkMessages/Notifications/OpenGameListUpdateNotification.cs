using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class OpenGameListUpdateNotification : NetworkMessageBase.NetworkMessageBase
	{
		public OpenGameListUpdateNotification(ClientId clientId, IEnumerable<NetworkGameInfo> openGames) 
			: base(NetworkMessageType.OpenGameListUpdateNotification, clientId)
		{
			OpenGames = openGames;
		}

		public IEnumerable<NetworkGameInfo> OpenGames { get; }

		public override string AsString()
		{
			var sb = new StringBuilder();

			foreach (var game in OpenGames)
			{
				sb.Append(game.GameId);
				sb.Append(",");
				sb.Append(game.InitiatorName);
				sb.Append(",");
				sb.Append(game.GameName);
				sb.Append(";");
			}

			if (OpenGames.Any())
				sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}

		public static OpenGameListUpdateNotification Parse (ClientId clientId, string s)
		{
			var games = new List<NetworkGameInfo>();

			if (string.IsNullOrWhiteSpace(s))
				return new OpenGameListUpdateNotification(clientId, games);

			var gameInfos = s.Split(';');

			foreach (var gameInfo in gameInfos)
			{
				var parts = gameInfo.Split(',');

				games.Add(new NetworkGameInfo(new NetworkGameId(Guid.Parse(parts[0])), parts[1], parts[2]));
			}

			return new OpenGameListUpdateNotification(clientId, games);			
		}
	}
}