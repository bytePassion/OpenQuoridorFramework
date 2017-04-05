using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OQF.Tournament.Contracts.DTO;
using OQF.Tournament.Contracts.Logger;

namespace OQF.Tournament.Application
{
	public class DataLogger : IDataLogger
	{
		public event Action<LogEntry> OnNewLogEntry;

		private readonly IDictionary<TournamentParticipant, ParticipantData> participantData;
		private readonly IList<LogEntry> logEntries;

		public DataLogger()
		{
			participantData = new Dictionary<TournamentParticipant, ParticipantData>();
			logEntries = new List<LogEntry>();
		}

		public void AddParticipant(TournamentParticipant participant)
		{
			if (!participantData.ContainsKey(participant))
			{
				participantData.Add(participant, new ParticipantData(participant));
			}
		}

		public void ReportMoveTime(TournamentParticipant participant, TimeSpan moveTime)
		{
			if (participantData.ContainsKey(participant))
			{
				participantData[participant].ReportMoveTime(moveTime);
			}
		}

		public void ReportMoveCount(TournamentParticipant participant, int moveCount, bool win)
		{
			if (participantData.ContainsKey(participant))
			{
				participantData[participant].ReportMoveCount(moveCount, win);
			}
		}

		public void ReportWallCount(TournamentParticipant participant, int wallCount, bool win)
		{
			if (participantData.ContainsKey(participant))
			{
				participantData[participant].ReportWallCount(wallCount, win);
			}
		}

		public void ReportResult(TournamentParticipant participant, bool win)
		{
			if (participantData.ContainsKey(participant))
			{
				participantData[participant].ReportResult(win);
			}
		}

		public void AnalyzeDataAndSaveToFile(string filename)
		{
			var sb = new StringBuilder();

			foreach (var participantDataValue in participantData.Values)
			{
				sb.AppendLine(participantDataValue.GetReport());
			}

			File.WriteAllText(filename, sb.ToString());
		}

		public void ReportLog(string msg, LogLevel logLevel)
		{
			var newEntry = new LogEntry(msg, logLevel);
			logEntries.Add(newEntry);
			OnNewLogEntry?.Invoke(newEntry);
		}

		public void SaveLogHistoryToFile(string filename)
		{
			var sb = new StringBuilder();

			foreach (var logEntry in logEntries)
			{
				sb.AppendLine(logEntry.LogMessage);
			}

			File.WriteAllText(filename, sb.ToString());
		}

		public IEnumerable<LogEntry> GetCompleteLogHistory()
		{
			return logEntries;
		}

		public void ClearLogger()
		{
			participantData.Clear();
			logEntries.Clear();
		}
	}
}
