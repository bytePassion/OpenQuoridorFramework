using System;
using System.Collections.Generic;
using System.Linq;
using OQF.Tournament.Contracts;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Services
{
	public class DeathMatch : ITournamentProcess
    {
        private List<TournamentParticipant> participants;		
		private List<MatchData> matches;
	    private List<TournamentParticipant> winners;

	    private int gameIndex;


	    public event Action<IList<Tuple<TournamentParticipant, int>>> ResultsAvailable;

	    public void Init(TournamentType tournamentType,
						 IEnumerable<TournamentParticipant> tournamentParticipantsparticipants)
        {
			winners = new List<TournamentParticipant>();
            participants = tournamentParticipantsparticipants.ToList();	
			
			matches = new List<MatchData>();

	        for(int i=0; i<participants.Count; i++)
	        {
		        for (int j = 0; j < participants.Count; j++)
		        {
			        if (i != j)
				    {
				        matches.Add(new MatchData(participants[i], participants[j]));
			        }
		        }
	        }

	        gameIndex = 0;
        }       

        public void ReportMatchWinner(TournamentParticipant winner)
        {
			winners.Add(winner);

	        if (!NextMatchNeeded())
	        {
		        var results = new Dictionary<TournamentParticipant, int>();

		        foreach (var tournamentParticipant in participants)
		        {
			        results.Add(tournamentParticipant, 0);
		        }

		        foreach (var w in winners)
		        {
			        results[w]++;
		        }

		        ResultsAvailable?.Invoke(results.OrderByDescending(kvp => kvp.Value)
											    .Select(kvp => new Tuple<TournamentParticipant, int>(kvp.Key, kvp.Value))
												.ToList());
	        }
        }

        public bool NextMatchNeeded()
        {
	        return gameIndex < matches.Count;
        }

        public MatchData GetNextMatchPlayers()
        {
	        return NextMatchNeeded() 
						? matches[gameIndex++] 
						: null;
        }
	   
    }
}