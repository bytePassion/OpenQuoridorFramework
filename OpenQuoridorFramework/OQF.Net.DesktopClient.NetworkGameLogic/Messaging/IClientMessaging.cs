using System;
using OQF.Net.LanMessaging.NetworkMessageBase;

namespace OQF.Net.DesktopClient.NetworkGameLogic.Messaging
{
	public interface IClientMessaging : IDisposable
	{
		event Action<NetworkMessageBase> NewIncomingMessage;
		void SendMessage (NetworkMessageBase msg);
	}
}
