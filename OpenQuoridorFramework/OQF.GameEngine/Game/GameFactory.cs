using OQF.Contest.Contracts.GameElements;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts;

namespace OQF.GameEngine.Game
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
