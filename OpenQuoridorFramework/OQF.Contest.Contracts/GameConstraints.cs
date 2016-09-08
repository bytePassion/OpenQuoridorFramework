using System;

namespace OQF.Contest.Contracts
{
	public class GameConstraints
	{
		public GameConstraints(TimeSpan maximalComputingTimePerMove, int maximalMovesPerPlayer)
		{
			MaximalComputingTimePerMove = maximalComputingTimePerMove;
			MaximalMovesPerPlayer = maximalMovesPerPlayer;
		}

		public TimeSpan MaximalComputingTimePerMove { get; }
		public int MaximalMovesPerPlayer { get; }
	}
}
