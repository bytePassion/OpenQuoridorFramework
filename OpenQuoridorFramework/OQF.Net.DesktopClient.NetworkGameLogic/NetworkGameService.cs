using System;
using System.Collections.Generic;
using System.Threading;
using Lib.Communication.State;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic.Messaging;
using OQF.Net.LanMessaging;
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

		public event Action<ConnectionStatus> ConnectionStatusChanged;
		public event Action<GameStatus> GameStatusChanged;
		public event Action<IDictionary<NetworkGameId, string>> UpdatedGameListAvailable;
		public event Action JoinError;
		public event Action<string> JoinSuccessful;
		public event Action<string> OpendGameIsStarting;		
		public event Action<bool, WinningReason> GameOver;
		public event Action<BoardState> NewBoardStateAvailable;

		private IClientMessaging messagingService;
		private ClientId clientId;
		private BoardState currentBoardState;

		private Timer connectionTimeoutTimer;
		private ConnectionStatus currentConnectionStatus;
		private GameStatus currentGameStatus;

		public NetworkGameService(ISharedStateWriteOnly<bool> isBoardRotatedVariable)
		{
			this.isBoardRotatedVariable = isBoardRotatedVariable;
			CurrentBoardState = null;
			CurrentGameId = null;
			TopPlayer = null;
			BottomPlayer = null;
			ClientPlayer = null;
			OpponendPlayer = null;
			OpendGameId = null;

			CurrentConnectionStatus = ConnectionStatus.NotConnected;		
			CurrentGameStatus = GameStatus.NoGame;	
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
		
		private NetworkGameId OpendGameId { get; set; }

		public string PlayerName     { get; private set; }
		public string GameName       { get; private set; }
		public Player TopPlayer      { get; private set; }
		public Player BottomPlayer   { get; private set; }
		public Player ClientPlayer   { get; private set; }
		public Player OpponendPlayer { get; private set; }

		public GameStatus CurrentGameStatus
		{
			get { return currentGameStatus; }
			private set
			{
				currentGameStatus = value;
				GameStatusChanged?.Invoke(CurrentGameStatus);
			}
		}

		public ConnectionStatus CurrentConnectionStatus
		{
			get { return currentConnectionStatus; }
			set
			{
				currentConnectionStatus = value;
				ConnectionStatusChanged?.Invoke(CurrentConnectionStatus);
			}
		}


		public void ConnectToServer(AddressIdentifier serverAddress, string playerName)
		{
			if (CurrentConnectionStatus == ConnectionStatus.NotConnected)
			{
				PlayerName = playerName;
				clientId = new ClientId(Guid.NewGuid());
				messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), clientId);
				messagingService.NewIncomingMessage += OnNewIncomingMessage;

				connectionTimeoutTimer = new Timer(OnConnectionTimeout, 
												   null, 
												   TimeSpan.FromMilliseconds(MessagingConstants.Timeout.ClientConnectionTimeout), 
												   TimeSpan.FromSeconds(1));

				CurrentConnectionStatus = ConnectionStatus.TryingToConnect;

				messagingService.SendMessage(new ConnectToServerRequest(clientId, playerName));
			}		
		}

		private void OnConnectionTimeout(object state)
		{
			if (CurrentConnectionStatus != ConnectionStatus.Connected)
			{
				Disconnect();
				CurrentConnectionStatus = ConnectionStatus.NotConnected;
			}
				
			connectionTimeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);
			connectionTimeoutTimer.Dispose();
			connectionTimeoutTimer = null;
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
			{
				OpendGameId = gameId;
				CurrentGameStatus = GameStatus.WaitingForOponend;				
				messagingService.SendMessage(new CreateGameRequest(clientId, gameName, gameId));
			}				
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
			OpendGameId = null;

			if (clientId != null)
				messagingService.SendMessage(new JoinGameRequest(clientId, gameId));
		}

		public void LeaveGame()
		{
			if (CurrentGameId != null)
			{
				CurrentGameStatus = GameStatus.NoGame;
				messagingService.SendMessage(new LeaveGameRequest(clientId, CurrentGameId));
			}
		}

		public void CancelCreatedGame()
		{
			if (clientId != null && OpendGameId != null)
			{
				messagingService.SendMessage(new CancelCreatedGameRequest(clientId, OpendGameId));
			}
			else
			{
				throw new Exception();
			}			
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
					CurrentConnectionStatus = ConnectionStatus.Connected;					
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

						CurrentGameStatus = GameStatus.PlayingJoinedGame;
						JoinSuccessful?.Invoke(msg.OpponendPlayerName);
					}
					else
					{
						CurrentGameStatus = GameStatus.NoGame;
						CurrentGameId = null;
						JoinError?.Invoke();
					}

					break;
				}
				case NetworkMessageType.GameOverNotification:
				{
					var msg = (GameOverNotification) incommingMsg;
					
					CurrentGameStatus = GameStatus.GameOver;
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

					CurrentGameStatus = GameStatus.PlayingOpendGame;
					OpendGameIsStarting?.Invoke(msg.OpponendPlayerName);

					break;
				}	
				case NetworkMessageType.CancelCreatedGameResponse:
				{
					var msg = (CancelCreatedGameResponse) incommingMsg;

					if (msg.ActionSuccessful)
					{
						CurrentGameStatus = GameStatus.NoGame;
					}

					break;
				}		
			}
		}

		public void Disconnect()
		{
			CurrentConnectionStatus = ConnectionStatus.NotConnected;
			CurrentGameStatus = GameStatus.NoGame;
			clientId = null;
			messagingService?.Dispose();
		}		
	}
}
