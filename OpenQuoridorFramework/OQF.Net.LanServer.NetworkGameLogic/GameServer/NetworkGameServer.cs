using System;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.Messaging;

namespace OQF.Net.LanServer.NetworkGameLogic.GameServer
{
	public class NetworkGameServer : INetworkGameServer
	{
		public event Action<string> NewOutputAvailable;

		private IServerMessaging messagingService;

		private readonly IClientRepository clientRepository;

		public NetworkGameServer(IClientRepository clientRepository)
		{
			this.clientRepository = clientRepository;
		}


		public void Activate(Address serverAddress)
		{
			if (messagingService != null)
			{
				Deactivate();	
			}

			clientRepository.ClearRepository();
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
