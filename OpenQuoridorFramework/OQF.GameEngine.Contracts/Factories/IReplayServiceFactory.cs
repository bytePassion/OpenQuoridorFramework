using OQF.GameEngine.Contracts.Replay;

namespace OQF.GameEngine.Contracts.Factories
{
	public interface IReplayServiceFactory
	{
		IReplayService CreateReplayService();
	}
}
