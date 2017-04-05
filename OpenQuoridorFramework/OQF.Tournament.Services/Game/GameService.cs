using System;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Tournament.Contracts;
using OQF.Tournament.Contracts.DTO;
using OQF.Tournament.Contracts.Logger;
using OQF.Utils.Enum;

namespace OQF.Tournament.Services.Game
{
	public class GameService : DisposingObject, IGameService
    {
		public event Action<TournamentParticipant, QProgress, WinningReason> WinnerAvailable;
        
        private readonly IProcessService processService;
	    private readonly IDataLogger dataLogger;

	    private TournamentGame currentGame;
	    private BoardState currentBoardState;

	    public GameService(IProcessService processService, IDataLogger dataLogger)
        {           
	        this.processService = processService;
	        this.dataLogger = dataLogger;

	        CurrentBoardState = null;
        }

	    public void StartGame(GameConstraints constraints, TournamentParticipant bottomParticipant, TournamentParticipant topParticipant)
	    {
		    if (currentGame == null)
		    {
				currentGame = new TournamentGame(processService, dataLogger);

				currentGame.NewBoardStateAvailable += OnNewBoardStateAvailable;				
				currentGame.WinnerAvailable        += OnWinnerAvailable;

				currentGame.StartGame(constraints, bottomParticipant, topParticipant);
			}
		    else
		    {
			    MessageBox.Show("internal error: tried to start 2nd parallel game");
		    }
	    }

	    private void OnWinnerAvailable(TournamentParticipant tournamentParticipant, QProgress qProgress, WinningReason arg3)
	    {
			DisposeGame();
			WinnerAvailable?.Invoke(tournamentParticipant, qProgress, arg3);			
	    }

	   private void OnNewBoardStateAvailable(BoardState boardState)
	    {
		    CurrentBoardState = boardState;			
	    }

	    public void AbortGame()
        {
           DisposeGame();
        }

	    private void DisposeGame()
	    {
		    if (currentGame != null)
		    {
			    CurrentBoardState = null;

				currentGame.NewBoardStateAvailable -= OnNewBoardStateAvailable;				
				currentGame.WinnerAvailable        -= OnWinnerAvailable;

				currentGame.Dispose();
			    currentGame = null;
		    }
	    }

	    protected override void CleanUp()
	    {
			DisposeGame();
	    }

	    public event Action<BoardState> NewBoardStateAvailable;

	    public BoardState CurrentBoardState
	    {
		    get { return currentBoardState; }
		    private set
		    {
			    currentBoardState = value;
			    NewBoardStateAvailable?.Invoke(CurrentBoardState);
		    }
	    }
    }
}