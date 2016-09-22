using System;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using SimpleWalkingBot.Graph;

namespace SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    
	    private PlayerType startPosition; 
	  
	    private static int counter = 0;

	    public void Init(PlayerType yourStartPosition, GameConstraints gameConstraints)
	    {
		    startPosition = yourStartPosition;
	    }

	    public void DoMove(BoardState currentState)
	    {
		    var target = startPosition == PlayerType.BottomPlayer 
								? YField.Nine
								: YField.One;
		  
			DebugMessageAvailable?.Invoke($"beginne Bewegungsberechnung [{counter++}]");

		    var nextMove = ComputeNextMove(currentState, target);

			NextMoveAvailable?.Invoke(nextMove);							
	    }

	    private Move ComputeNextMove(BoardState currentState, YField target)
	    {
			var graph = new XGraph(currentState, startPosition);

			var nextPosition = graph.GetNextPositionToMove(target);

			return nextPosition.HasValue
						? (Move) new FigureMove  (nextPosition.Value)
						: (Move) new Capitulation();
		}
    }
}
