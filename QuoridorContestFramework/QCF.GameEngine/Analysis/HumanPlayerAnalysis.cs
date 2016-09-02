using System.Collections.Generic;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.GameEngine.Contracts;

namespace QCF.GameEngine.Analysis
{
	internal class HumanPlayerAnalysis : IHumanPlayerAnalysis
	{
		private readonly BoardState boardState;

		public HumanPlayerAnalysis(BoardState boardState)
		{
			this.boardState = boardState;
		}

		public IEnumerable<Wall> GetPossibleWalls()
		{
			// TODO: implementation
			return new List<Wall>();
		}

		public IEnumerable<FieldCoordinate> GetPossibleMoves()
		{
			// TODO: implementation
			return new List<FieldCoordinate>();
		}
	}
}
