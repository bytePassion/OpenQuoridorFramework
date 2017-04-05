using System;

namespace OQF.Tournament.Communication.Messages
{
	public static class NetworkMessageCoding
	{
		private class MessageParts
		{
            public MessageParts(string messageAsString)
            {
                var indexOfFirstPipe = messageAsString.IndexOf("|", StringComparison.Ordinal);
                var typeAsString = messageAsString.Substring(0, indexOfFirstPipe);
                var content = messageAsString.Substring(indexOfFirstPipe + 1);

                Type = (NetworkMessageType)Enum.Parse(typeof(NetworkMessageType), typeAsString);
                Content = content;
            }

            public NetworkMessageType Type { get; }
			public string Content { get; }
		}

		public static string Encode(NetworkMessageBase msg)
		{
			return $"{msg.Type}|{msg.AsString()}";
		}
		
		public static NetworkMessageBase Decode(string messageString)
		{
			if (string.IsNullOrWhiteSpace(messageString))
				return null;

			var messageParts = new MessageParts(messageString);

			switch (messageParts.Type)
			{
				case NetworkMessageType.InitGameRequest:      return InitGameRequest.Parse(messageParts.Content);	
                case NetworkMessageType.NextMoveRequest:      return NextMoveRequest.Parse(messageParts.Content);
                case NetworkMessageType.LoadBotRequest:       return LoadBotRequest.Parse(messageParts.Content);
                case  NetworkMessageType.InitGameResponse:    return InitGameResponse.Parse(messageParts.Content);
                case NetworkMessageType.LoadBotResponse:      return LoadBotResponse.Parse(messageParts.Content);
                case NetworkMessageType.NextMoveResponse:     return NextMoveResponse.Parse(messageParts.Content);
				case NetworkMessageType.GameFinishedRequest:  return GameFinishedRequest.Parse(messageParts.Content);
				case NetworkMessageType.GameFinishedResponse: return GameFinishedResponse.Parse(messageParts.Content);

				default:
					throw new ArgumentException();
			}
		}

		
				
	}
}
