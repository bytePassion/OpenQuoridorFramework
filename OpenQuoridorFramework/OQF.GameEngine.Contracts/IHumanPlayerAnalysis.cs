using System.Collections.Generic;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IHumanPlayerAnalysis
	{
		IEnumerable<Wall>            GetPossibleWalls();
		IEnumerable<FieldCoordinate> GetPossibleMoves();
	}
}
