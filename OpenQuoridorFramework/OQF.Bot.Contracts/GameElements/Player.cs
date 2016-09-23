using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.GameElements
{
	/// <summary>
	/// The Player-Class contains all the game-invariant Player-informations
	/// </summary>

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
