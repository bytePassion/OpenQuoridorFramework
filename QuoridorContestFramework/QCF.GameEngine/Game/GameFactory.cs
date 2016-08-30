using QCF.GameEngine.Contracts;

namespace QCF.GameEngine.Game
{
	public class GameFactory : IGameFactory
	{
		public IGame CreateNewGame(string botDllFile)
		{
			return new LocalGamePvC(botDllFile);
		}
	}
}
