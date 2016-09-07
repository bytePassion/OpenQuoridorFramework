using OQF.Contest.Contracts.GameElements;

namespace OQF.Contest.Contracts.Moves
{
	public abstract class Move
	{
		protected Move(BoardState stateBeforeMove, Player playerAtMove)
		{
			StateBeforeMove = stateBeforeMove;
			PlayerAtMove = playerAtMove;
		}

		public BoardState StateBeforeMove { get; }
		public Player PlayerAtMove { get; }

		public abstract override string ToString();
	}
}
