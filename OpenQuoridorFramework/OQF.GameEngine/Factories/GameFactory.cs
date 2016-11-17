using OQF.Bot.Contracts;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Contracts.Games;
using OQF.GameEngine.Game;
using OQF.Utils.ProgressUtils;

namespace OQF.GameEngine.Factories
{
	public class GameFactory : IGameFactory
	{
		public IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, QProgress intitialProgress)
		{
			return new LocalGamePvB(uninitializedBot, botName, gameConstraints, intitialProgress);
		}

		public IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot,GameConstraints gameConstraints)
		{
			return new LocalGameBvB(uninitializedTopPlayerBot, uninitializedBottomPlayerBot, gameConstraints);
		}	
	}
}
