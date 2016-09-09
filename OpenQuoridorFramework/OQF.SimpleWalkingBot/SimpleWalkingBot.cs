using System;
using System.Threading;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace OQF.SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    private Player myself;
	    private PlayerType myPlayerType;

	    public void Init(Player yourPlayer, GameConstraints gameConstraints)
	    {
		    myself = yourPlayer;
		    myself.Name = "behindiBot";
		    myPlayerType = myself.PlayerType;
	    }

	    private static int counter = 0;

	    public void DoMove(BoardState currentState)
	    {
		    var myState = myPlayerType == PlayerType.BottomPlayer 
								? currentState.BottomPlayer 
								: currentState.TopPlayer;

		    var movingOffset = myPlayerType == PlayerType.BottomPlayer 
								? -1 
								: +1 ;

			DebugMessageAvailable?.Invoke($"bin am moooooven :) [{counter++}]");
				
			new Thread(() =>
			{
				Thread.Sleep(5000);
				NextMoveAvailable?.Invoke(new FigureMove(currentState,
													 myself,
													 new FieldCoordinate(myState.Position.XCoord,
																		 myState.Position.YCoord + movingOffset)));
			}).Start();								
	    }
    }
}
