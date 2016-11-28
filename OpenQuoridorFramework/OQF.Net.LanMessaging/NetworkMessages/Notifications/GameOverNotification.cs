using System;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;
using OQF.Utils.Enum;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class GameOverNotification : NetworkMessageBase.NetworkMessageBase
	{
		public GameOverNotification(ClientId clientId, bool win, WinningReason winningReason) 
			: base(NetworkMessageType.GameOverNotification, clientId)
		{
			Win = win;
			WinningReason = winningReason;
		}

		public bool Win { get; }
		public WinningReason WinningReason { get; }

		public override string AsString()
		{
			return $"{Win};{WinningReason}";
		}

		public static GameOverNotification Parse(ClientId clientId, string s)
		{
			var parts = s.Split(';');

			var win = bool.Parse(parts[0]);
			var winningReason = (WinningReason) Enum.Parse(typeof(WinningReason), parts[1]);

			return new GameOverNotification(clientId, win, winningReason);
		}
	}
}