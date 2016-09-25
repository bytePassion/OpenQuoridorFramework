using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, string intitialProgress);
		IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot, GameConstraints gameConstraints);

		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
