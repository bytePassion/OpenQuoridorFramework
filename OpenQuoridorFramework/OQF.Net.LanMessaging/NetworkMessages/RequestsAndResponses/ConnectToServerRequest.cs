using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ConnectToServerRequest : NetworkMessageBase.NetworkMessageBase
	{		
		public ConnectToServerRequest(ClientId clientId, string playerName)
			: base(NetworkMessageType.ConnectToServerRequest, clientId)
		{			
			PlayerName = playerName;
		}
				
		public string PlayerName { get; }

		public override string AsString()
		{
			return PlayerName;
        }

		public static ConnectToServerRequest Parse(ClientId clientId, string s)
		{			
			return new ConnectToServerRequest(clientId, s);
		}
	}
}
