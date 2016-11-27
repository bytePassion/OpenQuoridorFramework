using System;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;

namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public static class NetworkMessageCoding
	{
		public static string Encode(NetworkMessageBase msg)
		{
			return msg.Type + "|" +  msg.AsString();
		}
		
		public static NetworkMessageBase Decode(string messageString)
		{
			if (string.IsNullOrWhiteSpace(messageString))
				return null;

			var type = (NetworkMessageType)Enum.Parse(typeof(NetworkMessageType), GetTypeFromMsg(messageString));
			var msg = GetMsgContent(messageString);

			switch (type)
			{
				case NetworkMessageType.ErrorResponse:                       return ErrorResponse.Parse(msg);																			     			
				case NetworkMessageType.ConnectToServerRequest:              return ConnectToServerRequest.Parse(msg);
				case NetworkMessageType.ConnectToServerResponse:             return ConnectToServerResponse.Parse(msg);				
																		   
 				case NetworkMessageType.NewBoardStateAvailableNotification:                return NewBoardStateAvailableNotification.Parse(msg);
				
				default:
					throw new ArgumentException();
			}
		}

		private static string GetTypeFromMsg(string messageString)
		{
			var index = messageString.IndexOf("|", StringComparison.Ordinal);

			if (index == -1)
				throw new ArgumentException("inner error @ message decoding");

			return messageString.Substring(0, index);
		}

		private static string GetMsgContent(string messageString)
		{
			var index = messageString.IndexOf("|", StringComparison.Ordinal);

			if (index == -1)
				throw new ArgumentException("inner error @ message decoding");

			return messageString.Substring(index + 1, messageString.Length - index - 1);
		}
	}
}
