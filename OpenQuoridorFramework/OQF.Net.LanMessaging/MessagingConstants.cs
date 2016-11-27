using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.LanMessaging
{
	public static class MessagingConstants
	{
		public static class TcpIpPort
		{
			public static readonly IpPort PubSubPort   = new IpPort(6670);
			public static readonly IpPort PushPullPort = new IpPort(6671);
		}

		public static class Timeout
		{
			public const uint StandardSendingTimeout = 2000;
		}
	}
}
