namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public enum NetworkMessageType
	{
		ConnectToServerRequest,
		ConnectToServerResponse,
		JoinGameRequest,
		JoinGameResponse,
		CancelCreatedGameRequest,
		CancelCreatedGameResponse,
		CreateGameRequest,
		CreateGameResponse,		
				
		NewGameStateAvailableNotification,
		OpenGameListUpdateNotification,
		GameOverNotification,
		NextMoveSubmission,
		OpendGameIsStarting,
		ClientDisconnect,
		ServerDisconnect,
		LeaveGame,
		HeartBeat
	}
}
