using System;
using System.Collections.Generic;
using OQF.Bot.Contracts;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Contracts
{
	public interface ITournament : IDisposable
    {
        event Action<List<TournamentParticipant>> TournamentOver;

        void StartTournament(IEnumerable<TournamentParticipant> participants, GameConstraints gameConstraints, TournamentType tournamentType);
        void AbortTournament();
    }
}
