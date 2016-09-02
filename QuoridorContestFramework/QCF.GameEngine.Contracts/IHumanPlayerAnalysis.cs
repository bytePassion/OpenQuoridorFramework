using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;

namespace QCF.GameEngine.Contracts
{
	public interface IHumanPlayerAnalysis
	{
		IEnumerable<Wall>            GetPossibleWalls();
		IEnumerable<FieldCoordinate> GetPossibleMoves();
	}
}
