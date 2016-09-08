using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IGame CreateNewGame(IQuoridorBot uninitializedBot, GameConstraints gameConstraints);
		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
