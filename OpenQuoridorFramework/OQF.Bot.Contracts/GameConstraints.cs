using System;

namespace OQF.Bot.Contracts
{
	/// <summary>
	/// The GameConstraints class contains the time- and moveCount-contraints for a game
	/// </summary>

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
