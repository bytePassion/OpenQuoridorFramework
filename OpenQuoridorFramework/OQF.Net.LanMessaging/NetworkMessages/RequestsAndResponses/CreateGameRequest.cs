using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class CreateGameRequest : NetworkMessageBase.NetworkMessageBase
	{
		public CreateGameRequest(ClientId clientId, string gameName) 
			: base(NetworkMessageType.CreateGameRequest, clientId)
		{
			GameName = gameName;			
		}

		public string GameName { get; }
		

		public override string AsString()
		{
			return GameName;
		}

		public static CreateGameRequest Parse(ClientId clientId, string s)
		{
			return new CreateGameRequest(clientId, s);
		}
	}
}