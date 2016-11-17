using OQF.Bot.Contracts;
using OQF.GameEngine.Contracts.Games;
using OQF.Utils.ProgressUtils;

namespace OQF.GameEngine.Contracts.Factories
{
	public interface IGameFactory
	{
		IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, QProgress intitialProgress);
		IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot, GameConstraints gameConstraints);		
	}
}
