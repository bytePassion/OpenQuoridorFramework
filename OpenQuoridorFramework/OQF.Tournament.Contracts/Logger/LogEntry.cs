namespace OQF.Tournament.Contracts.Logger
{
	public class LogEntry
	{
		public LogEntry(string logMessage, LogLevel logLevel)
		{
			LogMessage = logMessage;
			LogLevel = logLevel;
		}

		public string LogMessage { get; }
		public LogLevel LogLevel { get; }
	}
}