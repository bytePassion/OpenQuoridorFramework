using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class NewGameStateAvailableNotification : NetworkMessageBase.NetworkMessageBase
	{
		public NewGameStateAvailableNotification (ClientId receiver, QProgress newGameState) 
			: base(NetworkMessageType.NewBoardStateAvailableNotification, receiver)
		{
			NewGameState = newGameState;
		}
				
		public QProgress NewGameState { get; }
		
		public override string AsString()
		{
			return NewGameState.Compressed;
		}

		public static NewGameStateAvailableNotification Parse (ClientId clientId, string s)
		{			
			return new NewGameStateAvailableNotification(clientId, CreateQProgress.FromCompressedProgressString(s));
		}
	}
}
