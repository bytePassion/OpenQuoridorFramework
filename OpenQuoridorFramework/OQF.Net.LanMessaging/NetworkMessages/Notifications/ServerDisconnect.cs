using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class ServerDisconnect : NetworkMessageBase.NetworkMessageBase
	{
		public ServerDisconnect ()
			: base(NetworkMessageType.ServerDisconnect, new ClientId(Guid.Empty))
		{
		}

		public override string AsString ()
		{
			return "----";
		}

		public static ServerDisconnect Parse (ClientId clientId, string s)
		{
			return new ServerDisconnect();
		}
	}
}