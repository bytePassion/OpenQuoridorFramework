namespace QCF.Tools.ConcurrencyLib
{
	public interface IThread
	{
		void Run();
		void Stop();
		
		bool IsRunning { get; }
	}
}