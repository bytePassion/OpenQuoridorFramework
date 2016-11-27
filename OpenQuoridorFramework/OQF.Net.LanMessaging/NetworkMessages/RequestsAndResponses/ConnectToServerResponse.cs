using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ConnectToServerResponse : NetworkMessageBase.NetworkMessageBase
	{		
		public ConnectToServerResponse(ClientId clientId)
			: base(NetworkMessageType.ConnectToServerResponse, clientId)
		{			
		}
				
		public override string AsString()
		{
			return string.Empty;
        }

		public static ConnectToServerResponse Parse(ClientId clientId, string s)
		{						
			return new ConnectToServerResponse(clientId);
		}
	}
}
