namespace QCF.GameEngine.Contracts
{
	public interface IGameFactory
	{
		IGame CreateNewGame(string botDllFile, int maxMovesPerPlayer);
	}
}
