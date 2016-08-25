using QCF.GameEngine.GameElements;

namespace QCF.GameEngine.Moves
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
