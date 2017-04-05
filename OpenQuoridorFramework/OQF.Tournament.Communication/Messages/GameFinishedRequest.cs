namespace OQF.Tournament.Communication.Messages
{
	public class GameFinishedRequest : NetworkMessageBase
	{
		public GameFinishedRequest ()
			: base(NetworkMessageType.GameFinishedRequest)
		{
			
		}
		
		public override string AsString ()
		{
			return "---";
		}

		public static GameFinishedRequest Parse (string s)
		{
			return new GameFinishedRequest();
		}
	}
}