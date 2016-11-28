using System;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public static class NetworkMessageCoding
	{
		private class MessageParts
		{
			public MessageParts(string messageAsString)
			{
				var indexOfFirstPipe = messageAsString.IndexOf("|", StringComparison.Ordinal);
				var typeAsString = messageAsString.Substring(0, indexOfFirstPipe);
				var restOfTheMessage = messageAsString.Substring(indexOfFirstPipe+1);
				var indexOfSecondPipe = restOfTheMessage.IndexOf("|", StringComparison.Ordinal);
				var clientIdAsString = restOfTheMessage.Substring(0, indexOfSecondPipe);
				var content = restOfTheMessage.Substring(indexOfSecondPipe + 1);

				Type = (NetworkMessageType) Enum.Parse(typeof(NetworkMessageType), typeAsString);
				ClientId = new ClientId(Guid.Parse(clientIdAsString));
				Content = content;
			}

			public NetworkMessageType Type { get; }
			public ClientId ClientId { get; }
			public string Content { get; }
		}

		public static string Encode(NetworkMessageBase msg)
		{
			return $"{msg.Type}|{msg.ClientId}|{msg.AsString()}";
		}
		
		public static NetworkMessageBase Decode(string messageString)
		{
			if (string.IsNullOrWhiteSpace(messageString))
				return null;

			var messageParts = new MessageParts(messageString);

			switch (messageParts.Type)
			{
				case NetworkMessageType.ErrorResponse:                       return ErrorResponse.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.ConnectToServerRequest:              return ConnectToServerRequest.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.ConnectToServerResponse:             return ConnectToServerResponse.Parse(messageParts.ClientId, messageParts.Content);	
				case NetworkMessageType.CreateGameRequest:                   return CreateGameRequest.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.JoinGameRequest:                     return JoinGameRequest.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.JoinGameResponse:                    return JoinGameResponse.Parse(messageParts.ClientId, messageParts.Content);

				case NetworkMessageType.NewBoardStateAvailableNotification:  return NewBoardStateAvailableNotification.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.OpenGameListUpdateNotification:      return OpenGameListUpdateNotification.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.GameOverNotification:                return GameOverNotification.Parse(messageParts.ClientId, messageParts.Content);
				case NetworkMessageType.NextMoveSubmission:                  return NextMoveSubmission.Parse(messageParts.ClientId, messageParts.Content);

				default:
					throw new ArgumentException();
			}
		}

		
				
	}
}
