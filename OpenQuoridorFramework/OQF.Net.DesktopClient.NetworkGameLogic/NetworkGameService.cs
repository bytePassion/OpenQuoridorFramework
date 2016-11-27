using System;
using System.Collections.Generic;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic.Messaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.DesktopClient.NetworkGameLogic
{
	public class NetworkGameService : INetworkGameService
	{
		public event Action GotConnected;
		public event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;

		private IClientMessaging messagingService;
		private ClientId clientId;


		public void ConnectToServer(AddressIdentifier serverAddress, string playerName)
		{
			clientId = new ClientId(Guid.NewGuid());
			messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), clientId);
			messagingService.NewIncomingMessage += OnNewIncomingMessage;


			messagingService.SendMessage(new ConnectToServerRequest(clientId, playerName));
		}

		public void CreateGame(string gameName, NetworkGameId gameId)
		{
			if (clientId != null)
				messagingService.SendMessage(new CreateGameRequest(clientId, gameName, gameId));
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
				case NetworkMessageType.OpenGameListUpdateNotification:
				{
					var msg = (OpenGameListUpdateNotification) networkMessageBase;
					UpdatedGameListAvailable?.Invoke(msg.OpenGames);
					break;
				}
			}
		}

		public void Dissconnect()
		{
			clientId = null;
			messagingService?.Dispose();
		}
	}
}
