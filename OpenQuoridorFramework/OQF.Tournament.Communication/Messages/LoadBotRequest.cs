using System;
using OQF.Bot.Contracts.Coordination;

namespace OQF.Tournament.Communication.Messages
{
    public sealed class LoadBotRequest : NetworkMessageBase
    {
        public PlayerType PlayerSide { get; }
        public string DllPath { get; }


        public LoadBotRequest(PlayerType playerSide, string dllPath) : base(NetworkMessageType.LoadBotRequest)
        {
            PlayerSide = playerSide;
            DllPath = dllPath;
        }

        public override string AsString()
        {
            return $"{PlayerSide};{DllPath}";
        }

        public static LoadBotRequest Parse(string s)
        {
            var parts = s.Split(';');

            var playerType = (PlayerType) Enum.Parse(typeof(PlayerType), parts[0]);
            var path = parts[1];

            return new LoadBotRequest(playerType, path);
        }
    }
}