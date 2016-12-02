using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class ClientDisconnect : NetworkMessageBase.NetworkMessageBase
	{
		public ClientDisconnect(ClientId clientId) 
			: base(NetworkMessageType.ClientDisconnect, clientId)
		{
		}

		public override string AsString()
		{
			return "----";
		}

		public static ClientDisconnect Parse(ClientId clientId, string s)
		{
			return new ClientDisconnect(clientId);
		}
	}
}