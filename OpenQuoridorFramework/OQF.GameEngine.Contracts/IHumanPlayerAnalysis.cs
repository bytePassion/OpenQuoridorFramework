using System.Collections.Generic;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;

namespace OQF.GameEngine.Contracts
{
	public interface IHumanPlayerAnalysis
	{
		IEnumerable<Wall>            GetPossibleWalls();
		IEnumerable<FieldCoordinate> GetPossibleMoves();
	}
}
