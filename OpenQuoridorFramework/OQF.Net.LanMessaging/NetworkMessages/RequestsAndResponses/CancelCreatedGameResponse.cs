using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class CancelCreatedGameResponse : NetworkMessageBase.NetworkMessageBase
	{
		public CancelCreatedGameResponse(ClientId clientId, bool actionSuccessful) 
			: base(NetworkMessageType.CancelCreatedGameResponse, clientId)
		{
			ActionSuccessful = actionSuccessful;
		}

		public bool ActionSuccessful { get; }

		public override string AsString()
		{
			return ActionSuccessful.ToString();
		}

		public static CancelCreatedGameResponse Parse(ClientId clientId, string s)
		{
			return new CancelCreatedGameResponse(clientId, bool.Parse(s));
		}
	}
}