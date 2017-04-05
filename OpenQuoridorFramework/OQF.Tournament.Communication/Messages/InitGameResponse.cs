using System;

namespace OQF.Tournament.Communication.Messages
{
    public sealed class InitGameResponse : NetworkMessageBase
    {
        public bool IsSuccessful { get; }

        public InitGameResponse(bool isSuccessful) : base(NetworkMessageType.InitGameResponse)
        {
            IsSuccessful = isSuccessful;
        }

        public override string AsString()
        {
            return $"{IsSuccessful}";
        }

        public static InitGameResponse Parse(string message)
        {
            return new InitGameResponse(Boolean.Parse(message));
        }
    }
}
