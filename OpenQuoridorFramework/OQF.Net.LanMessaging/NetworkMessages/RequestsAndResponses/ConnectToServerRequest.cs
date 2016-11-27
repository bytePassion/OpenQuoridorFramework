using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses
{
	public class ConnectToServerRequest : NetworkMessageBase.NetworkMessageBase
	{		
		public ConnectToServerRequest(AddressIdentifier clientAdress, ClientId sender)
			: base(NetworkMessageType.ConnectToServerRequest)
		{
			ClientAdress = clientAdress;
		}

		public AddressIdentifier ClientAdress { get; }
		public string PlayerName { get; }

		public override string AsString()
		{
			return ClientAdress.ToString();
        }

		public static ConnectToServerRequest Parse(string s)
		{
			var clientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(s);
			return new ConnectToServerRequest(clientAddress, null);
		}
	}
}
