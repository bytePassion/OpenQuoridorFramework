using System;
using System.Collections.Generic;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.Moves;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic.Messaging;
using OQF.Net.LanMessaging.AddressTypes;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.NetworkMessages.Notifications;
using OQF.Net.LanMessaging.NetworkMessages.RequestsAndResponses;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;

namespace OQF.Net.DesktopClient.NetworkGameLogic
{
	public class NetworkGameService : INetworkGameService
	{
		public event Action GotConnected;
		public event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;
		public event Action JoinError;
		public event Action<string> JoinSuccessful;
		public event Action<QProgress> NewProgressAvailable;
		public event Action<bool, WinningReason> GameOver;

		private IClientMessaging messagingService;
		private ClientId clientId;
		private QProgress currentProgress;

		public NetworkGameService()
		{
			CurrentProgress = null;
			CurrentGameId = null;
		}

		public QProgress CurrentProgress
		{
			get { return currentProgress; }
			private set
			{
				currentProgress = value;
				NewProgressAvailable?.Invoke(CurrentProgress);
			}
		}

		public NetworkGameId CurrentGameId { get; private set; }		


		public void ConnectToServer(AddressIdentifier serverAddress, string playerName)
		{
			clientId = new ClientId(Guid.NewGuid());
			messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), clientId);
			messagingService.NewIncomingMessage += OnNewIncomingMessage;


			messagingService.SendMessage(new ConnectToServerRequest(clientId, playerName));
		}

		public void CreateGame(string gameName, NetworkGameId gameId)
		{
			CurrentGameId = null;
			CurrentProgress = null;

			if (clientId != null)
				messagingService.SendMessage(new CreateGameRequest(clientId, gameName, gameId));
		}

		public void JoinGame(NetworkGameId gameId)
		{
			CurrentGameId = null;
			CurrentProgress = null;

			if (clientId != null)
				messagingService.SendMessage(new JoinGameRequest(clientId, gameId));
		}

		public void SubmitMove(Move nextMove)
		{
			if (clientId != null && CurrentGameId != null)
			{
				messagingService.SendMessage(new NextMoveSubmission(clientId, nextMove, CurrentGameId));
			}
			else
			{
				throw new Exception();
			}
		}

		private void OnNewIncomingMessage(NetworkMessageBase incommingMsg)
		{
			switch (incommingMsg.Type)
			{
				case NetworkMessageType.ConnectToServerResponse:
				{
					GotConnected?.Invoke();
					break;
				}
				case NetworkMessageType.OpenGameListUpdateNotification:
				{
					var msg = (OpenGameListUpdateNotification) incommingMsg;
					UpdatedGameListAvailable?.Invoke(msg.OpenGames);
					break;
				}
				case NetworkMessageType.NewBoardStateAvailableNotification:
				{
					var msg = (NewBoardStateAvailableNotification) incommingMsg;

					if (msg.GameId == CurrentGameId)
						CurrentProgress = msg.NewGameState;
					else
						throw new Exception();

					break;
				}
				case NetworkMessageType.JoinGameResponse:
				{
					var msg = (JoinGameResponse) incommingMsg;

					if (msg.JoinSuccessful)
					{
						CurrentGameId = msg.GameId;
						JoinSuccessful?.Invoke(msg.OpponendPlayerName);
					}
					else
					{
						CurrentGameId = null;
						JoinError?.Invoke();
					}

					break;
				}
				case NetworkMessageType.GameOverNotification:
				{
					var msg = (GameOverNotification) incommingMsg;

					GameOver?.Invoke(msg.Win, msg.WinningReason);

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
