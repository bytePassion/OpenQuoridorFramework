using OQF.Bot.Contracts.GameElements;
using OQF.Net.LanMessaging.NetworkMessageBase;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class NewBoardStateAvailableNotification : NetworkMessageBase.NetworkMessageBase
	{
		public NewBoardStateAvailableNotification () 
			: base(NetworkMessageType.NewBoardStateAvailableNotification)
		{
						
		}
				
		public BoardState BoardState { get; }
		
		public override string AsString()
		{
			return $"";
		}

		public static NewBoardStateAvailableNotification Parse (string s)
		{
			// TODO
			return new NewBoardStateAvailableNotification();
		}
	}
}
