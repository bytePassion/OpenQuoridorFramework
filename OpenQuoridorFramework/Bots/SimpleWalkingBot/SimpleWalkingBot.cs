using System;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using SimpleWalkingBot.Graph;

namespace SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    private Player myself;	   

	    public void Init(Player yourPlayer, GameConstraints gameConstraints)
	    {
		    myself = yourPlayer;
		    myself.Name = nameof(SimpleWalkingBot);		   
	    }

	    private static int counter = 0;

	    public void DoMove(BoardState currentState)
	    {
		    var target = myself.PlayerType == PlayerType.BottomPlayer 
								? YField.Nine
								: YField.One;
		  
			DebugMessageAvailable?.Invoke($"beginne Bewegungsberechnung [{counter++}]");

		    var nextMove = ComputeNextMove(currentState, target);

			NextMoveAvailable?.Invoke(nextMove);							
	    }

	    private Move ComputeNextMove(BoardState currentState, YField target)
	    {
			var graph = new XGraph(currentState, myself.PlayerType);

			var nextPosition = graph.GetNextPositionToMove(target);

			return nextPosition.HasValue
						? (Move) new FigureMove  (nextPosition.Value)
						: (Move) new Capitulation();
		}
    }
}
