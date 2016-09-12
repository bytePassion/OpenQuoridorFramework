using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts;

namespace OQF.GameEngine.Game
{
	public class GameFactory : IGameFactory
	{
		public IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, GameConstraints gameConstraints)
		{
			return new LocalGamePvB(uninitializedBot, gameConstraints);
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
