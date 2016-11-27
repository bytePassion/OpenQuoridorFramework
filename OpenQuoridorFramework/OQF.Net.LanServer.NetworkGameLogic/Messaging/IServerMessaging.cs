using System;
using OQF.Net.LanMessaging.NetworkMessageBase;

namespace OQF.Net.LanServer.NetworkGameLogic.Messaging
{
	public interface IServerMessaging : IDisposable
	{
		event Action<NetworkMessageBase> NewIncomingMessage;		
		void SendMessage(NetworkMessageBase msg);
	}
}
