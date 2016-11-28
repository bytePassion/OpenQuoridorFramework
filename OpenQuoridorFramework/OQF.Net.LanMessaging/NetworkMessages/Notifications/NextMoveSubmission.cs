using System;
using OQF.AnalysisAndProgress.ProgressUtils.Parsing;
using OQF.Bot.Contracts.Moves;
using OQF.Net.LanMessaging.NetworkMessageBase;
using OQF.Net.LanMessaging.Types;

namespace OQF.Net.LanMessaging.NetworkMessages.Notifications
{
	public class NextMoveSubmission : NetworkMessageBase.NetworkMessageBase
	{
		public NextMoveSubmission(ClientId clientId, Move nextMove, NetworkGameId gameId) 
			: base(NetworkMessageType.NextMoveSubmission, clientId)
		{
			NextMove = nextMove;
			GameId = gameId;
		}

		public Move NextMove { get; }
		public NetworkGameId GameId { get; }

		public override string AsString()
		{
			return $"{NextMove};{GameId}";
		}

		public static NextMoveSubmission Parse(ClientId clientId, string s)
		{
			var parts = s.Split(';');

			var nextMove = MoveParser.GetMove(parts[0]);
			var gameId = new NetworkGameId(Guid.Parse(parts[1]));

			return new NextMoveSubmission(clientId, nextMove, gameId);
		}
	}
}