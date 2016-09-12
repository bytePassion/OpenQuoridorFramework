using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, GameConstraints gameConstraints);
		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
