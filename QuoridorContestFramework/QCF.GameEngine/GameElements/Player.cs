using QCF.GameEngine.Coordination;

namespace QCF.GameEngine.GameElements
{
	public class Player
	{
		public Player(PlayerType playerType, string name)
		{
			PlayerType = playerType;
			Name = name;
		}
		
		public string Name { get; }
		public PlayerType PlayerType { get; }
	}
}
