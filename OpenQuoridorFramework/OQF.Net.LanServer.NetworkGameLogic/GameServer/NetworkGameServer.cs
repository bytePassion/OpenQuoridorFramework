using System;
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

		private HeartbeatService heartbeatService;

		public NetworkGameServer(IClientRepository clientRepository,
								 IGameRepository gameRepository)
		{
			this.clientRepository = clientRepository;
			this.gameRepository = gameRepository;

			gameRepository.RepositoryChanged += OnRepositoryChanged;
		}

		private void OnRepositoryChanged()
		{
			SendGameListUpdateToAllClients();
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

			heartbeatService = new HeartbeatService(messagingService, clientRepository);
			heartbeatService.ClientVanished += OnClientVanished;

			NewOutputAvailable?.Invoke("ActivatedServer");
		}

		private void OnClientVanished(ClientId clientId)
		{
			DisconnectClient(clientId);
		}

		private void OnNewIncomingMessage(NetworkMessageBase newIncommingMsg)
		{
			switch (newIncommingMsg.Type)
			{
				case NetworkMessageType.ConnectToServerRequest:
				{
					var msg = (ConnectToServerRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< ConnectToServer from {msg.PlayerName}");

					if (!clientRepository.IsClientIdRegistered(msg.ClientId))
					{
						clientRepository.AddClient(msg.ClientId, msg.PlayerName);

						NewOutputAvailable?.Invoke($">>> ConnectToServerResponse to {msg.PlayerName}");

						messagingService.SendMessage(new ConnectToServerResponse(msg.ClientId));
						SendGameListUpdate(msg.ClientId);
					}
					else
					{
						NewOutputAvailable?.Invoke($">>> ConnectToServerResponse ERROR (@{msg.PlayerName})");						
					}

					break;
				}	
				case NetworkMessageType.CreateGameRequest:
				{					
					var msg = (CreateGameRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< CreateGameRequest from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					var newGameId = new NetworkGameId(Guid.NewGuid());

					while (gameRepository.GetGameById(newGameId) != null)
						newGameId = new NetworkGameId(Guid.NewGuid());

					gameRepository.CreateGame(newGameId, 
											  clientRepository.GetClientById(msg.ClientId), 
											  msg.GameName);

					NewOutputAvailable?.Invoke($">>> CreateGameResponse to {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					messagingService.SendMessage(new CreateGameResponse(msg.ClientId, newGameId));				

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
						NewOutputAvailable?.Invoke($">>> NextMoveSubmission ERROR! @({clientRepository.GetClientById(msg.ClientId).PlayerName} [{msg.NextMove}])");
					}
					else
					{
						game.ReportMove(clientRepository.GetClientById(msg.ClientId), msg.NextMove);
					}

					break;
				}
				case NetworkMessageType.LeaveGame:
				{
					var msg = (LeaveGame) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< LeaveGame from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					var game = gameRepository.GetGameById(msg.GameId);
					
					if (game == null || !game.IsGameActive)
					{
						NewOutputAvailable?.Invoke($">>> LeaveGame ERROR! (@{clientRepository.GetClientById(msg.ClientId).PlayerName})");
					}
					else
					{
						LeaveGame(msg.ClientId, game);
					}

					break;
				}
				case NetworkMessageType.CancelCreatedGameRequest:
				{
					var msg = (CancelCreatedGameRequest) newIncommingMsg;

					NewOutputAvailable?.Invoke($"<<< CancelCreatedGameRequest from {clientRepository.GetClientById(msg.ClientId).PlayerName}");

					var game = gameRepository.GetGameById(msg.GameId);

					if (game.IsGameActive)
					{
						NewOutputAvailable?.Invoke($"<<< CancelCreatedGameResponse (negativ) to {clientRepository.GetClientById(msg.ClientId).PlayerName}");
						messagingService.SendMessage(new CancelCreatedGameResponse(msg.ClientId, false));
					}
					else
					{
						gameRepository.DeleteGame(msg.GameId);
						NewOutputAvailable?.Invoke($"<<< CancelCreatedGameResponse (positiv) to {clientRepository.GetClientById(msg.ClientId).PlayerName}");
						messagingService.SendMessage(new CancelCreatedGameResponse(msg.ClientId, true));
					}

					break;
				}
				case NetworkMessageType.ClientDisconnect:
				{
					NewOutputAvailable?.Invoke($"<<< Disconnect from {clientRepository.GetClientById(newIncommingMsg.ClientId)?.PlayerName}");

					DisconnectClient(newIncommingMsg.ClientId);

					break;
				}
			}			
		}

		private void DisconnectClient(ClientId clientId)
		{
			var game = gameRepository.GetGameByPlayer(clientId);
			clientRepository.RemoveClient(clientId);

			if (game != null)
			{
				if (game.IsGameActive)
					LeaveGame(clientId, game);
				else
					gameRepository.DeleteGame(game.GameId);
			}			
		}

		private void LeaveGame(ClientId clientId, INetworkGame game)
		{
			game.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			game.WinnerAvailable        -= OnWinnerAvailable;

			NewOutputAvailable?.Invoke($">>> GameOver notification for ({game.GameName})");

			messagingService.SendMessage(new GameOverNotification(clientId == game.GameInitiator.ClientId
																					? game.Opponend.ClientId
																					: game.GameInitiator.ClientId,
																  true,
																  WinningReason.Capitulation));


			// TODO store game

			gameRepository.DeleteGame(game.GameId);
		}

		private void OnWinnerAvailable(INetworkGame networkGame, ClientInfo clientInfo, WinningReason winningReason)
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

		private void OnNewBoardStateAvailable(INetworkGame networkGame, BoardState boardState)
		{
			var newProgress = CreateQProgress.FromBoardState(boardState);

			NewOutputAvailable?.Invoke($">>> new boardState for {networkGame.GameInitiator.PlayerName} and {networkGame.Opponend.PlayerName}");


			messagingService.SendMessage(new NewGameStateAvailableNotification(networkGame.GameInitiator.ClientId, newProgress, networkGame.GameId));
			messagingService.SendMessage(new NewGameStateAvailableNotification(networkGame.Opponend.ClientId,      newProgress, networkGame.GameId));
		}

		private void SendGameListUpdateToAllClients()
		{
			var gameList = gameRepository.GetAllGames()
										 .Where(game => !game.IsGameActive)
										 .Select(game => new NetworkGameInfo(game.GameId,
																			 game.GameInitiator.PlayerName,
																			 game.GameName));

			var msg = new OpenGameListUpdateNotification(new ClientId(Guid.Empty), gameList);
			NewOutputAvailable?.Invoke(">>> GameListUpdate to all Players");
			messagingService.SendMessage(msg);
		}

		private void SendGameListUpdate(ClientId clientId)
		{
			var gameList = gameRepository.GetAllGames()
										 .Where(game => !game.IsGameActive)
										 .Select(game => new NetworkGameInfo(game.GameId, 
																			 game.GameInitiator.PlayerName, 
																			 game.GameName));

			var msg = new OpenGameListUpdateNotification(clientId, gameList);
			NewOutputAvailable?.Invoke($">>> GameListUpdate to {clientRepository.GetClientById(msg.ClientId).PlayerName}");
			messagingService.SendMessage(msg);
		}

		public void Deactivate()
		{
			NewOutputAvailable?.Invoke("DeactivatedServer");

			if (messagingService != null)
			{
				heartbeatService.Dispose();
				heartbeatService.ClientVanished -= OnClientVanished;

				messagingService.SendMessage(new ServerDisconnect());

				messagingService.NewIncomingMessage -= OnNewIncomingMessage;
				messagingService.Dispose();
				messagingService = null;
			}
		}
	}
}
