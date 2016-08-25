using QCF.GameEngine.Contracts.GameElements;

namespace QCF.GameEngine.Contracts.Moves
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
	}
}
