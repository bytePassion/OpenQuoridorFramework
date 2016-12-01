using System;
using System.Collections.Generic;
using System.Linq;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.GameElements;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;
using OQF.Net.LanServer.Contracts;
using OQF.Net.LanServer.NetworkGameLogic.Messaging;
using OQF.Utils.Enum;

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
						SendGameListUpdate(msg.ClientId);
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

					NewOutputAvailable?.Invoke($"<<< CreateGameRequest from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					gameRepository.CreateGame(msg.GameId, 
											  clientRepository.GetClientById(msg.ClientId), 
											  msg.GameName);

					SendGameListUpdateToAllClients();

					break;
				}
				case NetworkMessageType.JoinGameRequest:
				{
					var msg = (JoinGameRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< JoinGameRequest from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					var game = gameRepository.GetGameById(msg.GameId);

					if (game == null || game.IsGameActive)
					{
						NewOutputAvailable?.Invoke($">>> JoinGameResponse (negative) to {clientRepository.GetClientById(msg.ClientId).PlayerName}");
						messagingService.SendMessage(new JoinGameResponse(msg.ClientId, msg.GameId, false, ""));
					}
					else
					{
						NewOutputAvailable?.Invoke($">>> JoinGameResponse (positive) to {clientRepository.GetClientById(msg.ClientId).PlayerName}");

						messagingService.SendMessage(new JoinGameResponse(msg.ClientId, 
																		  game.GameId, 
																		  true, 
																		  game.GameInitiator.PlayerName));

						NewOutputAvailable?.Invoke($">>> Opend Game is starting for {game.GameInitiator.PlayerName}");

						messagingService.SendMessage(new OpendGameIsStarting(game.GameInitiator.ClientId, 
																			 game.GameId, 
																			 clientRepository.GetClientById(msg.ClientId).PlayerName));

						game.NewBoardStateAvailable += OnNewBoardStateAvailable;
						game.WinnerAvailable        += OnWinnerAvailable;

						game.StartGame(clientRepository.GetClientById(msg.ClientId));
					}

					break;
				}
				case NetworkMessageType.NextMoveSubmission:
				{
					var msg = (NextMoveSubmission) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< NextMoveSubmission from {clientRepository.GetClientById(msg.ClientId).PlayerName} [{msg.NextMove}]");

					var game = gameRepository.GetGameById(msg.GameId);

					if (game == null || !game.IsGameActive)
					{
						NewOutputAvailable?.Invoke(">>> NextMoveSubmission ERROR!");
					}
					else
					{
						game.ReportMove(clientRepository.GetClientById(msg.ClientId), msg.NextMove);
					}

					break;
				}
				case NetworkMessageType.LeaveGameRequest:
				{
					var msg = (LeaveGameRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< LeaveGameRequest from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					var game = gameRepository.GetGameById(msg.GameId);

					if (game == null || !game.IsGameActive)
					{
						NewOutputAvailable?.Invoke(">>> NextMoveSubmission ERROR!");
					}
					else
					{
						game.NewBoardStateAvailable -= OnNewBoardStateAvailable;
						game.WinnerAvailable        -= OnWinnerAvailable;

						messagingService.SendMessage(new GameOverNotification(msg.ClientId == game.GameInitiator.ClientId 
																					? game.Opponend.ClientId 
																					: game.GameInitiator.ClientId, 
																			  true, 
																			  WinningReason.Capitulation));


						// TODO store game

						gameRepository.DeleteGame(game.GameId);
					}

					break;
				}
			}			
		}

		private void OnWinnerAvailable(NetworkGame networkGame, ClientInfo clientInfo, WinningReason winningReason)
		{
			networkGame.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			networkGame.WinnerAvailable        -= OnWinnerAvailable;

			NewOutputAvailable?.Invoke($">>> GameOver notification for ({networkGame.GameName})");


			messagingService.SendMessage(new GameOverNotification(networkGame.GameInitiator.ClientId, 
																  networkGame.GameInitiator.ClientId == clientInfo.ClientId, 
																  winningReason));

			messagingService.SendMessage(new GameOverNotification(networkGame.Opponend.ClientId,
																  networkGame.Opponend.ClientId == clientInfo.ClientId, 
																  winningReason));

			// TODO store game

			gameRepository.DeleteGame(networkGame.GameId);
		}

		private void OnNewBoardStateAvailable(NetworkGame networkGame, BoardState boardState)
		{
			var newProgress = CreateQProgress.FromBoardState(boardState);

			NewOutputAvailable?.Invoke($">>> new boardState for {networkGame.GameInitiator.PlayerName} and {networkGame.Opponend.PlayerName}");


			messagingService.SendMessage(new NewGameStateAvailableNotification(networkGame.GameInitiator.ClientId, newProgress, networkGame.GameId));
			messagingService.SendMessage(new NewGameStateAvailableNotification(networkGame.Opponend.ClientId,      newProgress, networkGame.GameId));
		}

		private void SendGameListUpdateToAllClients()
		{			
			foreach (var client in clientRepository.GetAllClients())
			{				
				SendGameListUpdate(client.ClientId);
			}
		}

		private void SendGameListUpdate(ClientId clientId)
		{
			var gameList = new Dictionary<NetworkGameId, string>();

			foreach (var networkGame in gameRepository.GetAllGames().Where(game => !game.IsGameActive))
			{
				gameList.Add(networkGame.GameId, networkGame.GameName);
			}

			var msg = new OpenGameListUpdateNotification(clientId, gameList);
			NewOutputAvailable?.Invoke($">>> GameListUpdate to {clientRepository.GetClientById(msg.ClientId).PlayerName}");
			messagingService.SendMessage(msg);
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
