using System;
using QCF.Contest.Contracts;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QFC.SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    private Player myself;
	    private PlayerType myPlayerType;

	    public void Init(Player yourPlayer)
	    {
		    myself = yourPlayer;
		    myself.Name = "behindiBot";
		    myPlayerType = myself.PlayerType;
	    }

	    public void DoMove(BoardState currentState)
	    {
		    var myState = myPlayerType == PlayerType.BottomPlayer 
								? currentState.BottomPlayer 
								: currentState.TopPlayer;

		    var movingOffset = myPlayerType == PlayerType.BottomPlayer 
								? -1 
								: +1 ;

		    NextMoveAvailable?.Invoke(new FigureMove(currentState, 
													 myself, 
													 myState.Position, 
													 new FieldCoordinate(myState.Position.XCoord, 
																		 myState.Position.YCoord + movingOffset)));
	    }
    }
}
