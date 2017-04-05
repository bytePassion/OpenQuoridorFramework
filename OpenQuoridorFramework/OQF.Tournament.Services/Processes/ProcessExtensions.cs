using System.Diagnostics;

namespace OQF.Tournament.Services.Processes
{
	public static class ProcessExtensions
	{
		public static bool IsRunning (this Process process)
		{
			if (process == null)
				return false;

			try
			{
				Process.GetProcessById(process.Id);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}