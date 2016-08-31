using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCF.Contest.Contracts;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.XelorsBot
{
    public class Main : IQuoridorBot
    {
	    public event Action<Move> NextMoveAvailable;
	    public event Action<string> DebugMessageAvailable;

	    public void Init(Player yourPlayer)
	    {
		    throw new NotImplementedException();
	    }

	    public void DoMove(BoardState currentState)
	    {
		    throw new NotImplementedException();
	    }
    }
}
