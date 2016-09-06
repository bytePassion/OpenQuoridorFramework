using System;
using QCF.Contest.Contracts;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.XelorsBot
{
	public class Main : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    private Player mySelf;

	    public void Init(Player yourPlayer)
	    {
		    mySelf = yourPlayer;
	    }

	    public void DoMove(BoardState currentState)
	    {
			DebugMessageAvailable?.Invoke("");
		    NextMoveAvailable?.Invoke(new Capitulation(currentState, mySelf));
	    }
    }
}
