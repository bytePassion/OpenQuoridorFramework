using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Contracts.Games;
using OQF.GameEngine.Game;

namespace OQF.GameEngine.Factories
{
	public class GameFactory : IGameFactory
	{
		public IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, 
									  string intitialProgress, ProgressTextType progressTextType)
		{
			return new LocalGamePvB(uninitializedBot, botName, gameConstraints, intitialProgress, progressTextType);
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
