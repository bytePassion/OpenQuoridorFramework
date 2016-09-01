using QCF.Contest.Contracts.GameElements;

namespace QCF.Contest.Contracts.Moves
{
	public class Capitulation : Move
	{
		public Capitulation(BoardState stateBeforeMove, Player playerAtMove) 
			: base(stateBeforeMove, playerAtMove)
		{
		}
		
		public override string ToString() => "capitulation";
	}
}
