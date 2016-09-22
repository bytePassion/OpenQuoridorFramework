using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.GameElements
{
	public class Player
	{
		public Player(PlayerType playerType, string name = null)
		{
			PlayerType = playerType;
			Name = name;
		}
		
		public string Name { get; }
		public PlayerType PlayerType { get; }
	}
}
