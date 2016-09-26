using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Games;

namespace OQF.GameEngine.Contracts.Factories
{
	public interface IGameFactory
	{
		IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, string intitialProgress);
		IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot, GameConstraints gameConstraints);

		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
