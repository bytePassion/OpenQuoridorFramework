using System;
using OQF.Tournament.Communication.Messages;

namespace OQF.Tournament.Contracts
{
	public interface IMessagingService : IDisposable
    {
        event Action<NetworkMessageBase> NewMsgAvailable;
        void SendMessage(NetworkMessageBase msg);
    }
}