using OQF.Bot.Contracts.Coordination;

namespace OQF.Bot.Contracts.Moves
{
	/// <summary>
	/// With this move a player can move his pawn
	/// </summary>

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
