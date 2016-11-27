using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ErrorResponse : NetworkMessageBase.NetworkMessageBase
	{
		public ErrorResponse (string errorMessage, ClientId receiver) 
			: base(NetworkMessageType.ErrorResponse)
		{
			ErrorMessage = errorMessage;
		}
		
		public string ErrorMessage { get; } 

		public override string AsString()
		{
			return ErrorMessage;			
		}

		public static ErrorResponse Parse (string s)
		{			
			return new ErrorResponse(s, null);
		}
	}
}
