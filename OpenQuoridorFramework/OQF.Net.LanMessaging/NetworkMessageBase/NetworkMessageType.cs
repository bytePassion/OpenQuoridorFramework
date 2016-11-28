﻿namespace OQF.Net.LanMessaging.NetworkMessageBase
{
	public enum NetworkMessageType
	{
		ConnectToServerRequest,
		ConnectToServerResponse,
		JoinGameRequest,
		JoinGameResponse,

		CreateGameRequest,
		
		NewBoardStateAvailableNotification,
		OpenGameListUpdateNotification,

		ErrorResponse		
	}
}
