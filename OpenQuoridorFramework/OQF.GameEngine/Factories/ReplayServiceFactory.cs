using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Contracts.Replay;
using OQF.GameEngine.Replay;

namespace OQF.GameEngine.Factories
{
	public class ReplayServiceFactory : IReplayServiceFactory
	{
		public IReplayService CreateReplayService()
		{
			return new ReplayService();
		}
	}
}
