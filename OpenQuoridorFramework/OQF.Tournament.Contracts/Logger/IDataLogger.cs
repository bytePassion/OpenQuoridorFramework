using System;
using System.Collections.Generic;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Contracts.Logger
{
	public interface IDataLogger
	{
		event Action<LogEntry> OnNewLogEntry; 
		
		void AddParticipant (TournamentParticipant participant);
		void ReportMoveTime (TournamentParticipant participant, TimeSpan moveTime);
		void ReportMoveCount(TournamentParticipant participant, int moveCount, bool win);
		void ReportWallCount(TournamentParticipant participant, int wallCount, bool win);
		void ReportResult   (TournamentParticipant participant, bool win);

		void AnalyzeDataAndSaveToFile(string filename);

		void ReportLog(string msg, LogLevel logLevel);
		void SaveLogHistoryToFile(string filename);
		IEnumerable<LogEntry> GetCompleteLogHistory();

		void ClearLogger();
	}
}