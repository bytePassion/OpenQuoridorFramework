using System;
using System.Collections.Generic;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.Messaging;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class NetworkGameServer : INetworkGameServer
	{
		public event Action<string> NewOutputAvailable;

		private IServerMessaging messagingService;

		private readonly IClientRepository clientRepository;
		private readonly IGameRepository gameRepository;

		public NetworkGameServer(IClientRepository clientRepository,
								 IGameRepository gameRepository)
		{
			this.clientRepository = clientRepository;
			this.gameRepository = gameRepository;
		}


		public void Activate(Address serverAddress)
		{
			if (messagingService != null)
			{
				Deactivate();	
			}

			clientRepository.ClearRepository();
			gameRepository.ClearRepository();

			messagingService = new ServerMessaging(serverAddress);

			messagingService.NewIncomingMessage += OnNewIncomingMessage;

			NewOutputAvailable?.Invoke("ActivatedServer");
		}

		private void OnNewIncomingMessage(NetworkMessageBase newIncommingMsg)
		{
			switch (newIncommingMsg.Type)
			{
				case NetworkMessageType.ConnectToServerRequest:
				{
					var msg = (ConnectToServerRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< ConnectToServer from ({msg.PlayerName}|{msg.ClientId})");

					if (!clientRepository.IsClientIdRegistered(msg.ClientId))
					{
						clientRepository.AddClient(msg.ClientId, msg.PlayerName);

						NewOutputAvailable?.Invoke(">>> ConnectToServerResponse");

						messagingService.SendMessage(new ConnectToServerResponse(msg.ClientId));
					}
					else
					{
						NewOutputAvailable?.Invoke(">>> ConnectToServerResponse ERROR");
						// ERROR !!!!
					}

					break;
				}	
				case NetworkMessageType.CreateGameRequest:
				{
					var msg = (CreateGameRequest) newIncommingMsg;

					gameRepository.CreateGame(msg.GameId, 
											  clientRepository.GetClientById(msg.ClientId), 
											  msg.GameName);

					SendGameListUpdate();

					break;
				}
			}			
		}

		private void SendGameListUpdate()
		{

			var gameList = new Dictionary<NetworkGameId, string>();

			foreach (var networkGame in gameRepository.GetAllGames())
			{
				gameList.Add(networkGame.GameId, networkGame.GameName);
			}

			foreach (var client in clientRepository.GetAllClients())
			{				
				var msg = new OpenGameListUpdateNotification(client.ClientId, gameList);
				messagingService.SendMessage(msg);
			}
		}

		public void Deactivate()
		{
			NewOutputAvailable?.Invoke("DeactivatedServer");

			if (messagingService != null)
			{
				messagingService.NewIncomingMessage -= OnNewIncomingMessage;
				messagingService.Dispose();
				messagingService = null;
			}
		}
	}
}
