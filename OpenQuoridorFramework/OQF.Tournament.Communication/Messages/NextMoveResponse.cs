using System;
using OQF.AnalysisAndProgress.ProgressUtils.Parsing;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.Moves;

namespace OQF.Tournament.Communication.Messages
{
    public sealed class NextMoveResponse : NetworkMessageBase
    {
        public PlayerType Player { get; }
        public Move NextMove { get; }

        public NextMoveResponse(PlayerType player, Move nextMove) : base(NetworkMessageType.NextMoveResponse)
        {
            Player = player;
            NextMove = nextMove;
        }

        public override string AsString()
        {
            return $"{Player};{NextMove}";
        }

        public static NextMoveResponse Parse(string message)
        {
            var parts = message.Split(';');

            var nextMove = MoveParser.GetMove(parts[1]);
            var player = (PlayerType)Enum.Parse(typeof(PlayerType), parts[0]);

            return new NextMoveResponse(player, nextMove);
        }
    }
}
