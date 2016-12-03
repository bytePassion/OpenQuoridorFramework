using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class HeartBeat : NetworkMessageBase.NetworkMessageBase
	{
		public HeartBeat(ClientId clientId) 
			: base(NetworkMessageType.HeartBeat, clientId)
		{
		}

		public override string AsString()
		{
			return "----";
		}

		public static HeartBeat Parse(ClientId clientId, string s)
		{
			return new HeartBeat(clientId);
		}
	}
}
