using System.Collections.Generic;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.AnalysisAndProgress.Analysis
{
	public class HumanPlayerAnalysisResult
	{
		public HumanPlayerAnalysisResult(IEnumerable<Wall> possibleWalls, IEnumerable<FieldCoordinate> possibleMoves)
		{
			PossibleWalls = possibleWalls;
			PossibleMoves = possibleMoves;
		}

		public IEnumerable<Wall> PossibleWalls { get; }
		public IEnumerable<FieldCoordinate> PossibleMoves { get; }
	}
}