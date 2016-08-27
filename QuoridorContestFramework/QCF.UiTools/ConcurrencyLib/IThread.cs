namespace QCF.UiTools.ConcurrencyLib
{
	public interface IThread
	{
		void Run();
		void Stop();
		
		bool IsRunning { get; }
	}
}