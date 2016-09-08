using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts;

namespace OQF.GameEngine.Game
{
	public class GameFactory : IGameFactory
	{
		public IGame CreateNewGame(string botDllFile, GameConstraints gameConstraints)
		{
			return new LocalGamePvC(botDllFile, gameConstraints);
		}

		public IHumanPlayerAnalysis GetGameAnalysis(BoardState boardState)
		{
			return new HumanPlayerAnalysis(boardState);
		}
	}
}
