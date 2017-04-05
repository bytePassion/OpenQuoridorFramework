using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Tournament.Contracts;
using OQF.Tournament.Contracts.DTO;
using OQF.Tournament.Contracts.Logger;
using OQF.Utils.Enum;

namespace OQF.Tournament.Services
{
	public class Tournament : DisposingObject, ITournament
    {
		public event Action<List<TournamentParticipant>> TournamentOver;

		private readonly ITournamentProcess tournamentProcess;
	    private readonly IGameService gameService;
	    private readonly IDataLogger dataLogger;
	    private IEnumerable<TournamentParticipant> contestants;
	    private GameConstraints currentGameConstraints;        

        public Tournament(ITournamentProcess tournamentProcess, IGameService gameService, IDataLogger dataLogger)
        {	        
	        this.tournamentProcess = tournamentProcess;
	        this.gameService = gameService;
	        this.dataLogger = dataLogger;

	        gameService.WinnerAvailable += OnWinnerAvailable;            
			tournamentProcess.ResultsAvailable += OnResultsAvailable;
		}

        private void OnResultsAvailable(IList<Tuple<TournamentParticipant, int>> result)
	    {
		    for (int index = 0; index < result.Count; index++)
		    {
			    var tuple = result[index];
			    dataLogger.ReportLog($"{index+1}. {tuple.Item1.Name} ({tuple.Item2})", LogLevel.OnlyGameResults);
			}

		    TournamentOver?.Invoke(result.Select(tuple => tuple.Item1).ToList());
	    }

	    public void StartTournament(IEnumerable<TournamentParticipant> newContestants, GameConstraints constraints, TournamentType tournamentType)
        {
	        foreach (var tournamentParticipant in newContestants)
	        {
		        dataLogger.AddParticipant(tournamentParticipant);
	        }

	        currentGameConstraints = constraints;
            contestants = newContestants;
            tournamentProcess.Init(tournamentType, contestants);
            
            var nextPair = tournamentProcess.GetNextMatchPlayers();

            if (nextPair != null)
            {
                gameService.StartGame(constraints, nextPair.BottomPlayer, nextPair.TopPlayer);
                
            }           
        }

	    private void OnWinnerAvailable(TournamentParticipant arg1, QProgress gameProgress, WinningReason arg2)
	    {
		    tournamentProcess.ReportMatchWinner(arg1);

		    var progress = gameProgress == null ? "no progress" : gameProgress.Compressed;

			dataLogger.ReportLog($"{arg1.Name} wins because of {arg2} [{progress}]", LogLevel.OnlyGameResults);


			if (tournamentProcess.NextMatchNeeded())
		    {			    
				Thread.Sleep(1000);

			    var nextPair = tournamentProcess.GetNextMatchPlayers();
			    gameService.StartGame(currentGameConstraints, nextPair.BottomPlayer, nextPair.TopPlayer);
		    }		   
	    }

	    public void AbortTournament()
        {
            // TODO
        }

	    protected override void CleanUp()
	    {
			gameService.WinnerAvailable -= OnWinnerAvailable;
			tournamentProcess.ResultsAvailable -= OnResultsAvailable;
		}
    }
}