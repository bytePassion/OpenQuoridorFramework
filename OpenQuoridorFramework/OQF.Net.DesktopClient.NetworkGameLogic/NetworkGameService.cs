using System;
using System.Collections.Generic;
using Lib.Communication.State;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
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
		private readonly ISharedStateWriteOnly<bool> isBoardRotatedVariable;
		public event Action GotConnected;
		public event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;
		public event Action JoinError;
		public event Action<string> JoinSuccessful;
		public event Action<string> OpendGameIsStarting;		
		public event Action<bool, WinningReason> GameOver;
		public event Action<BoardState> NewBoardStateAvailable;

		private IClientMessaging messagingService;
		private ClientId clientId;
		private BoardState currentBoardState;

		public NetworkGameService(ISharedStateWriteOnly<bool> isBoardRotatedVariable)
		{
			this.isBoardRotatedVariable = isBoardRotatedVariable;
			CurrentBoardState = null;
			CurrentGameId = null;
			TopPlayer = null;
			BottomPlayer = null;
			ClientPlayer = null;
			OpponendPlayer = null;
		}


		public BoardState CurrentBoardState
		{
			get { return currentBoardState; }
			private set
			{
				currentBoardState = value;
				NewBoardStateAvailable?.Invoke(CurrentBoardState);
			}
		}

		

		public NetworkGameId CurrentGameId { get; private set; }

		public string PlayerName { get; private set; }
		public string GameName { get; private set; }

		public Player TopPlayer { get; private set; }
		public Player BottomPlayer { get; private set; }
		public Player ClientPlayer { get; private set; }
		public Player OpponendPlayer { get; private set; }


		

		public void ConnectToServer(AddressIdentifier serverAddress, string playerName)
		{
			PlayerName = playerName;
			clientId = new ClientId(Guid.NewGuid());
			messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), clientId);
			messagingService.NewIncomingMessage += OnNewIncomingMessage;


			messagingService.SendMessage(new ConnectToServerRequest(clientId, playerName));
		}

		public void CreateGame(string gameName, NetworkGameId gameId)
		{
			GameName = gameName;

			CurrentGameId = null;
			CurrentBoardState = null;
			TopPlayer = null;
			BottomPlayer = null;
			ClientPlayer = null;
			OpponendPlayer = null;

			if (clientId != null)
				messagingService.SendMessage(new CreateGameRequest(clientId, gameName, gameId));
		}

		public void JoinGame(NetworkGameId gameId, string gameName)
		{
			GameName = gameName;

			CurrentGameId = null;
			CurrentBoardState = null;
			TopPlayer = null;
			BottomPlayer = null;
			ClientPlayer = null;
			OpponendPlayer = null;

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
				case NetworkMessageType.NewGameStateAvailableNotification:
				{
					var msg = (NewGameStateAvailableNotification) incommingMsg;

					if (msg.GameId == CurrentGameId)
						CurrentBoardState = msg.NewGameState.GetBoardState(BottomPlayer, TopPlayer);
					else
						throw new Exception();

					break;
				}
				case NetworkMessageType.JoinGameResponse:
				{
					var msg = (JoinGameResponse) incommingMsg;

					if (msg.JoinSuccessful)
					{
						isBoardRotatedVariable.Value = true;
						CurrentGameId = msg.GameId;

						TopPlayer      = new Player(PlayerType.TopPlayer,    PlayerName);
						BottomPlayer   = new Player(PlayerType.BottomPlayer, msg.OpponendPlayerName);
						ClientPlayer   = TopPlayer;
						OpponendPlayer = BottomPlayer;

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
				case NetworkMessageType.OpendGameIsStarting:
				{
					var msg = (OpendGameIsStarting) incommingMsg;

					isBoardRotatedVariable.Value = false;
					CurrentGameId = msg.GameId;

					TopPlayer      = new Player(PlayerType.TopPlayer,    msg.OpponendPlayerName);
					BottomPlayer   = new Player(PlayerType.BottomPlayer, PlayerName);
					ClientPlayer   = BottomPlayer;
					OpponendPlayer = TopPlayer;

					OpendGameIsStarting?.Invoke(msg.OpponendPlayerName);

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
