using System.Collections.Generic;
using OQF.Application.Tournament.ViewModels;

namespace OQF.Application.Tournament.Services
{
    public interface ITournamentService
    {
        void StartTournament(IEnumerable<BotData> contestants);

    }
}
