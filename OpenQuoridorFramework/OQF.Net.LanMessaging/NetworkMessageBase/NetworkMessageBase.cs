using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public abstract class NetworkMessageBase
	{
		protected NetworkMessageBase(NetworkMessageType type, ClientId clientId)
		{
			Type = type;
			ClientId = clientId;
		}
		
		public NetworkMessageType Type { get; }
		public ClientId ClientId { get; }

		public abstract string AsString();
	}
}
