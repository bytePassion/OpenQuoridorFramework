using OQF.Contest.Contracts.Coordination;

namespace OQF.Contest.Contracts.Moves
{
	public class FigureMove : Move
	{
		public FigureMove(FieldCoordinate newPosition)			
		{			
			NewPosition = newPosition;
		}
		
		public FieldCoordinate NewPosition { get; }
		public override string ToString() => NewPosition.ToString();
	}
}
