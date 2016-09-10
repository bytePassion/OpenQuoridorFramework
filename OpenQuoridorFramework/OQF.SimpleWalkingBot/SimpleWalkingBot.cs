using System;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.SimpleWalkingBot.Graph;

namespace OQF.SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    private Player myself;	   

	    public void Init(Player yourPlayer, GameConstraints gameConstraints)
	    {
		    myself = yourPlayer;
		    myself.Name = "behindiBot";		   
	    }

	    private static int counter = 0;

	    public void DoMove(BoardState currentState)
	    {
		    var target = myself.PlayerType == PlayerType.BottomPlayer 
								? YField.Nine
								: YField.One;
		  
			DebugMessageAvailable?.Invoke($"bin am moooooven :) [{counter++}]");
				
			var graph = new XGraph(currentState, myself.PlayerType);
			NextMoveAvailable?.Invoke(new FigureMove(currentState,myself,graph.GetNextPositionToMove(target)));							
	    }
    }
}
