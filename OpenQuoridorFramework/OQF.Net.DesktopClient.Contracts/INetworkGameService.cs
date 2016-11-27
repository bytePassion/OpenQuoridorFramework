﻿using System;
using OQF.Net.LanMessaging.AddressTypes;

namespace OQF.Net.DesktopClient.Contracts
{
	public interface INetworkGameService
	{
		event Action GotConnected;

		void ConnectToServer(AddressIdentifier serverAddress);
	}
}
