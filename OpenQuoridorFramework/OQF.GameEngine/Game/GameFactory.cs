using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts;

namespace OQF.GameEngine.Game
{
	public class GameFactory : IGameFactory
	{
		public IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, string intitialProgress)
		{
			return new LocalGamePvB(uninitializedBot, botName, gameConstraints, intitialProgress);
		}

		public IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot,GameConstraints gameConstraints)
		{
			return new LocalGameBvB(uninitializedTopPlayerBot, uninitializedBottomPlayerBot, gameConstraints);
		}

		public IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState)
		{
			return new HumanPlayerAnalysis(boardState);
		}
	}
}
