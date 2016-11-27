﻿using System;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic.Messaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.NetworkGameLogic
{
	public class NetworkGameService : INetworkGameService
	{
		public event Action GotConnected;

		private IClientMessaging messagingService;

		public void ConnectToServer(AddressIdentifier serverAddress)
		{
			var newClientId = new ClientId(Guid.NewGuid());
			messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), newClientId);
			messagingService.NewIncomingMessage += OnNewIncomingMessage;


			messagingService.SendMessage(new ConnectToServerRequest(newClientId, "xelor"));
		}

		private void OnNewIncomingMessage(NetworkMessageBase networkMessageBase)
		{
			switch (networkMessageBase.Type)
			{
				case NetworkMessageType.ConnectToServerResponse:
				{
					GotConnected?.Invoke();
					break;
				}
			}
		}

		public void Dissconnect()
		{
			messagingService?.Dispose();
		}
	}
}
