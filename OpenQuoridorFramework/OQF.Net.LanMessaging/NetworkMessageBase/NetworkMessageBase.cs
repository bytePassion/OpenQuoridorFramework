namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public abstract class NetworkMessageBase
	{
		protected NetworkMessageBase(NetworkMessageType type)
		{
			Type = type;			
		}
		
		public NetworkMessageType Type { get; }

		public abstract string AsString();
	}
}
