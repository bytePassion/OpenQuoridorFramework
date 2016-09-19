using System;
using OQF.Contest.Contracts;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;

namespace XelorsBot
{
	public class Main : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    //private Player mySelf;

	    public void Init(Player yourPlayer, GameConstraints gameConstraints)
	    {
		   // mySelf = yourPlayer;
	    }

	    public void DoMove(BoardState currentState)
	    {			
		    NextMoveAvailable?.Invoke(new Capitulation());
	    }
    }
}
