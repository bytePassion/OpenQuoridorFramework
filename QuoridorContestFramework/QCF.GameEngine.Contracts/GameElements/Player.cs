using QCF.GameEngine.Contracts.Coordination;

namespace QCF.GameEngine.Contracts.GameElements
{
	public class Player
	{
		public Player(PlayerType playerType, string name = null)
		{
			PlayerType = playerType;
			Name = name;
		}
		
		public string Name { get; set; }
		public PlayerType PlayerType { get; }
	}
}
