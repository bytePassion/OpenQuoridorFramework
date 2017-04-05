namespace OQF.Tournament.Communication.Messages
{
	public class GameFinishedResponse : NetworkMessageBase
	{
		public GameFinishedResponse()
			: base(NetworkMessageType.GameFinishedResponse)
		{
			
		}

		public override string AsString()
		{
			return "---";
		}

		public static GameFinishedResponse Parse(string s)
		{
			return new GameFinishedResponse();
		}
	}
}