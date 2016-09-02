using QCF.Contest.Contracts.GameElements;
using QCF.GameEngine.Analysis;
using QCF.GameEngine.Contracts;

namespace QCF.GameEngine.Game
{
	public class GameFactory : IGameFactory
	{
		public IGame CreateNewGame(string botDllFile, int maxMovesPerPlayer)
		{
			return new LocalGamePvC(botDllFile, maxMovesPerPlayer);
		}

		public IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState)
		{
			return new HumanPlayerAnalysis(boardState);
		}
	}
}
