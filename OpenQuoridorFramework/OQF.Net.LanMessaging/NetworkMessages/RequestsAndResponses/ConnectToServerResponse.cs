using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ConnectToServerResponse : NetworkMessageBase.NetworkMessageBase
	{		
		public ConnectToServerResponse(ClientId clientId)
			: base(NetworkMessageType.ConnectToServerResponse)
		{			
		}
				

		public override string AsString()
		{
			return "";
        }

		public static ConnectToServerResponse Parse(string s)
		{						
			return new ConnectToServerResponse(new ClientId(Guid.Parse(s)));
		}
	}
}
