using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ErrorResponse : NetworkMessageBase.NetworkMessageBase
	{
		public ErrorResponse (ClientId receiver, string errorMessage) 
			: base(NetworkMessageType.ErrorResponse, receiver)
		{
			ErrorMessage = errorMessage;
		}
		
		public string ErrorMessage { get; } 

		public override string AsString()
		{
			return ErrorMessage;			
		}

		public static ErrorResponse Parse (ClientId clientId, string s)
		{			
			return new ErrorResponse(clientId, s);
		}
	}
}
