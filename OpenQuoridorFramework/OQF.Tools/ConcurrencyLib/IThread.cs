namespace OQF.Tools.ConcurrencyLib
{
	public interface IThread
	{
		void Run();
		void Stop();
		
		bool IsRunning { get; }
	}
}