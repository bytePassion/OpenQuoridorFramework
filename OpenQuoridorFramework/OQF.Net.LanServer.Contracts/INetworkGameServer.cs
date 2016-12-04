using System;
using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.LanServer.Contracts
{
	public interface INetworkGameServer
	{
		event Action<string> NewOutputAvailable;

		void Activate(Address serverAddress);
		void Deactivate();
	}
}
