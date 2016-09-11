using OQF.Contest.Contracts.GameElements;

namespace OQF.Contest.Contracts.Moves
{
	public class Capitulation : Move
	{
		public Capitulation(BoardState stateBeforeMove, Player playerAtMove) 
			: base(stateBeforeMove, playerAtMove)
		{
		}
		
		public override string ToString() => "cap";
	}
}
