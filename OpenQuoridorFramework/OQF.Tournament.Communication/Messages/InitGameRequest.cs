using System;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;

namespace OQF.Tournament.Communication.Messages
{
	public class InitGameRequest : NetworkMessageBase
	{
	    public InitGameRequest(PlayerType playerType, GameConstraints constraints)
			: base(NetworkMessageType.InitGameRequest)
	    {
	        Constraints = constraints;
	        PlayerType = playerType;
	    }
				
		public PlayerType PlayerType { get; }

	    public GameConstraints Constraints { get;  }

	    public override string AsString()
	    {
	        return $"{PlayerType};{Constraints.MaximalMovesPerPlayer};{Constraints.MaximalComputingTimePerMove}";
	    }

	    public static InitGameRequest Parse(string s)
	    {
	        var parts = s.Split(';');

	        var playerType = (PlayerType) Enum.Parse(typeof(PlayerType), parts[0]);
            var maxMoves = int.Parse(parts[1]);
	        var thinkingTime = TimeSpan.Parse(parts[2]);

            return new InitGameRequest(playerType, new GameConstraints(thinkingTime, maxMoves));
		}
	}
}
