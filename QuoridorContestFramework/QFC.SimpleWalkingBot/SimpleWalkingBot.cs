using System;
using QCF.GameEngine.Contracts;
using QCF.GameEngine.Coordination;
using QCF.GameEngine.GameElements;
using QCF.GameEngine.Moves;

namespace QFC.SimpleWalkingBot
{
	public class SimpleWalkingBot : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;

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
		    var myState = myPlayerType == PlayerType.BottomPlayer ? currentState.BottomPlayer : currentState.TopPlayer;

		    var movingOffset = myPlayerType == PlayerType.BottomPlayer ? -1 : +1 ;


		    NextMoveAvailable?.Invoke(new FigureMove(currentState, 
													 myself, 
													 myState.Position, 
													 new FieldCoordinate(myState.Position.XCoord, 
																		 myState.Position.YCoord + movingOffset)));
	    }
    }
}
