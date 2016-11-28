using System;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class NewBoardStateAvailableNotification : NetworkMessageBase.NetworkMessageBase
	{
		public NewBoardStateAvailableNotification (ClientId receiver, QProgress newGameState, NetworkGameId gameId) 
			: base(NetworkMessageType.NewBoardStateAvailableNotification, receiver)
		{
			NewGameState = newGameState;
			GameId = gameId;
		}
				
		public QProgress NewGameState { get; }
		public NetworkGameId GameId { get; }
		
		public override string AsString()
		{
			return $"{NewGameState.Compressed};{GameId}";
		}

		public static NewBoardStateAvailableNotification Parse (ClientId clientId, string s)
		{
			var parts = s.Split(';');

			var progress = CreateQProgress.FromCompressedProgressString(parts[0]);
			var gameId = new NetworkGameId(Guid.Parse(parts[1]));

			return new NewBoardStateAvailableNotification(clientId, progress, gameId);
		}
	}
}
