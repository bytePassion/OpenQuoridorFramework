using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.GameEngine.Contracts.Games;

namespace OQF.GameEngine.Contracts.Factories
{
	public interface IGameFactory
	{
		IPvBGame CreateNewGame(IQuoridorBot uninitializedBot, string botName, GameConstraints gameConstraints, QProgress intitialProgress);
		IBvBGame CreateNewGame(IQuoridorBot uninitializedBottomPlayerBot, IQuoridorBot uninitializedTopPlayerBot, GameConstraints gameConstraints);		
	}
}
