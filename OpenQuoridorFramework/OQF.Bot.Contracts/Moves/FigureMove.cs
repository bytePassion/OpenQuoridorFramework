using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.Moves
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
