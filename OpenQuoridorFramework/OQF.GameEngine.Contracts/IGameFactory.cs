using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IGame CreateNewGame(string botDllFile, GameConstraints gameConstraints);
		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
