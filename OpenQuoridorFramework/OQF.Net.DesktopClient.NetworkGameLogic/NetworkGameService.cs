using System;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.DesktopClient.NetworkGameLogic
{
	public class NetworkGameService : INetworkGameService
	{
		public event Action GotConnected;

		public void ConnectToServer(AddressIdentifier serverAddress)
		{
			
		}
	}
}
