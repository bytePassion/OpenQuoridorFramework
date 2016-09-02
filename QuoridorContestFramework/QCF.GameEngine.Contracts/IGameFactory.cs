using QCF.Contest.Contracts.GameElements;

namespace QCF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IGame CreateNewGame(string botDllFile, int maxMovesPerPlayer);
		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
