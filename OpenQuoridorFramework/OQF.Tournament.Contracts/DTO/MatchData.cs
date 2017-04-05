using System;

namespace OQF.Tournament.Contracts.DTO
{
	public class MatchData
    {
        public TournamentParticipant BottomPlayer { get; }
        public TournamentParticipant TopPlayer { get; }
        public Guid GameId { get; }

        public MatchData(TournamentParticipant bottomParticipant, TournamentParticipant topParticipant)
        {
            GameId = Guid.NewGuid();
            BottomPlayer = bottomParticipant;
            TopPlayer = topParticipant;
        }
    }
}