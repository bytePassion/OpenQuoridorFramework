using System;
using System.Collections.Generic;
using System.Linq;
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
		public event Action<IEnumerable<NetworkGameInfo>> UpdatedGameListAvailable;
		public event Action JoinError;		
		public event Action<bool, WinningReason> GameOver;
		public event Action<BoardState> NewBoardStateAvailable;

		private IClientMessaging messagingService;
		//private HeartbeatService heartbeatService;
		
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
		private ClientId ClientId { get; set; }

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
				ClientId = new ClientId(Guid.NewGuid());
				messagingService = new ClientMessaging(new Address(new TcpIpProtocol(), serverAddress), ClientId);
				messagingService.NewIncomingMessage += OnNewIncomingMessage;

				connectionTimeoutTimer = new Timer(OnConnectionTimeout, 
												   null, 
												   TimeSpan.FromMilliseconds(MessagingConstants.Timeout.ClientConnectionTimeout), 
												   TimeSpan.FromSeconds(1));

				CurrentConnectionStatus = ConnectionStatus.TryingToConnect;

				messagingService.SendMessage(new ConnectToServerRequest(ClientId, playerName));
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

		public void CreateGame(string gameName)
		{
			GameName = gameName;

			CurrentGameId = null;
			CurrentBoardState = null;
			TopPlayer = null;
			BottomPlayer = null;
			ClientPlayer = null;
			OpponendPlayer = null;

			if (ClientId != null)
			{
				messagingService.SendMessage(new CreateGameRequest(ClientId, gameName));
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

			if (ClientId != null)
				messagingService.SendMessage(new JoinGameRequest(ClientId, gameId));
		}

		public void LeaveGame()
		{
			if (CurrentGameId != null)
			{
				CurrentGameStatus = GameStatus.NoGame;
				CurrentBoardState = null;
				messagingService.SendMessage(new LeaveGame(ClientId, CurrentGameId));
			}
		}

		public void CancelCreatedGame()
		{
			if (ClientId != null && OpendGameId != null)
			{
				messagingService.SendMessage(new CancelCreatedGameRequest(ClientId, OpendGameId));
			}
			else
			{
				throw new Exception();
			}			
		}

		public void SubmitMove(Move nextMove)
		{
			if (ClientId != null && CurrentGameId != null)
			{
				messagingService.SendMessage(new NextMoveSubmission(ClientId, nextMove, CurrentGameId));
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
					
					//heartbeatService = new HeartbeatService(messagingService, ClientId);
					//heartbeatService.ServerVanished += OnServerVanished;
								
					break;
				}
				case NetworkMessageType.OpenGameListUpdateNotification:
				{
					var msg = (OpenGameListUpdateNotification) incommingMsg;
					UpdatedGameListAvailable?.Invoke(msg.OpenGames
														.Where(gameInfo => gameInfo.GameId != OpendGameId && gameInfo.GameId != CurrentGameId));
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
					}
					else
					{
						CurrentGameStatus = GameStatus.NoGame;
						CurrentBoardState = null;
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
					break;
				}	
				case NetworkMessageType.CancelCreatedGameResponse:
				{
					var msg = (CancelCreatedGameResponse) incommingMsg;

					if (msg.ActionSuccessful)
					{
						CurrentGameStatus = GameStatus.NoGame;
						CurrentBoardState = null;
					}

					break;
				}	
				case NetworkMessageType.ServerDisconnect:
				{
					Disconnect();
					break;
				}	
				case NetworkMessageType.CreateGameResponse:
				{
					var msg = (CreateGameResponse) incommingMsg;

					OpendGameId = msg.GameId;
					CurrentGameStatus = GameStatus.WaitingForOponend;

					break;
				}
			}
		}

		private void OnServerVanished()
		{
			Disconnect();
		}

		public void Disconnect()
		{
			if (CurrentConnectionStatus == ConnectionStatus.Connected)
			{
				//heartbeatService.ServerVanished -= OnServerVanished;
				//heartbeatService.Dispose();
				messagingService.SendMessage(new ClientDisconnect(ClientId));
			}

			CurrentConnectionStatus = ConnectionStatus.NotConnected;
			CurrentGameStatus = GameStatus.NoGame;
			CurrentBoardState = null;
			ClientId = null;			

			messagingService?.Dispose();
		}		
	}
}
