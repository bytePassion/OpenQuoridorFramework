using OQF.Contest.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IGame CreateNewGame(string botDllFile, int maxMovesPerPlayer);
		IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState);
	}
}
