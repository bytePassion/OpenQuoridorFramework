namespace Lib.Concurrency
{
	public interface IThread
	{
		void Run();
		void Stop();
		
		bool IsRunning { get; }
	}

	
	/*
	 
	public class ExampleThread : IThread
	{
		private volatile bool stopRunning;
	
		public ExampleThread ()
		{						
		}
	
		public void Run ()
		{
			IsRunning = true;			

			while (!stopRunning)
			{
				// DO Stuff
			}

			IsRunning = false;			
		}

	

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}	 
	 
	 */
}