using System;
using System.Collections.Generic;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Contracts
{
	/// <summary>
	/// das hier wird von ITournament verwendet um die spiel-parungen zu finden.
	/// man übergibt die art des tourniers und die spieler
	/// dann kann man fragen "so, und wer soll jetzt zuerst gegeneinander antreten?"
	/// --> man bekommt die nächste paarung
	/// mit diesen informationen geht man zum IGameService und startet ein einzelnens spiel.
	/// ist das spiel fertig, berichtet man dem ITournamentProcess wer gewonnen hat und fragt
	/// nach der nächsten paarung .. usw.
	/// 
	/// die Liste von GetResults ist vom ersten zum letzten platz sortiert
	/// (ACHTUNG: GetResults sollte erst aufgerufen werden, wenn es keine weiteren matches zu spielen gibt)
	/// 
	/// </summary>


	public interface ITournamentProcess
	{
		event Action<IList<Tuple<TournamentParticipant, int>>> ResultsAvailable;
		
        void Init(TournamentType tournamentType, IEnumerable<TournamentParticipant> participants);
        void ReportMatchWinner(TournamentParticipant winner); 
        bool NextMatchNeeded();
        MatchData GetNextMatchPlayers();	    
    }
}