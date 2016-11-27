namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public enum NetworkMessageType
	{
		ConnectToServerRequest,
		ConnectToServerResponse,

		CreateGameRequest,
		
		NewBoardStateAvailableNotification,
		OpenGameListUpdateNotification,

		ErrorResponse		
	}
}
