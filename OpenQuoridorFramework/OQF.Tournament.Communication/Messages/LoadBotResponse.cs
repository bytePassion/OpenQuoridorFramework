using System;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.Tournament.Communication.Messages
{
    public sealed class LoadBotResponse : NetworkMessageBase
    {
        public Player Computerplayer { get; }

        public LoadBotResponse(Player computerplayer) : base(NetworkMessageType.LoadBotResponse)
        {
            Computerplayer = computerplayer;
        }

        public override string AsString()
        {
            return $"{Computerplayer.Name};{Computerplayer.PlayerType}";
        }

        public static LoadBotResponse Parse(string message)
        {
            var parts = message.Split(';');
            var playerType = (PlayerType) Enum.Parse((typeof(PlayerType)), parts[1]);
            var playerName = parts[0];

            return new LoadBotResponse(new Player(playerType, playerName));
        }
    }
}
