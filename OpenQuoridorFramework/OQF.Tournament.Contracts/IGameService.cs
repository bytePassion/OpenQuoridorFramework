using System;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.CommonUiElements.Board.ViewModels;
using OQF.Tournament.Contracts.DTO;
using OQF.Utils.Enum;

namespace OQF.Tournament.Contracts
{
	/// <summary>
	///     hier wird ein einzelnen spiel abgehandelt ...
	///     mittels des IProcessService kann k�nnen die prozesse f�r die bots erstellt werden
	///     hauptaufgabe hier ist - neben der ablaufkoordination - die �berwachung der zeiten
	/// </summary>
	public interface IGameService : IDisposable, IBoardStateProvider
    {
		event Action<TournamentParticipant, QProgress, WinningReason> WinnerAvailable;

        void StartGame(GameConstraints constraints, TournamentParticipant bottomParticipant, TournamentParticipant topParticipant);
        void AbortGame();
    }
}