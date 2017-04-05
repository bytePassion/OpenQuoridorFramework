using System.Collections.Generic;

namespace OQF.Tournament.Contracts.DTO
{
	public class TournamentResult
	{
		public IDictionary<int, TournamentParticipant> Positionings { get; }
	}
}