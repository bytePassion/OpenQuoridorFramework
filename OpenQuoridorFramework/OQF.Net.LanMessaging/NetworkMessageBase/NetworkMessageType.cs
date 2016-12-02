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


		LeaveGameRequest,
		CreateGameRequest,
		
		NewGameStateAvailableNotification,
		OpenGameListUpdateNotification,
		GameOverNotification,
		NextMoveSubmission,
		OpendGameIsStarting,

		ErrorResponse		
	}
}
