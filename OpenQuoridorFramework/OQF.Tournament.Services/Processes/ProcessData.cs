using System;
using System.Diagnostics;
using System.Threading;
using OQF.Tournament.Contracts;

namespace OQF.Tournament.Services.Processes
{
	public class ProcessData
    {
        public ProcessData(IMessagingService messagingService,  Guid internalProcessId, Process process, string port)
        {
            MessagingService = messagingService;
            InternalProcessId = internalProcessId;
            Process = process;
            Port = port;
        }

        public IMessagingService MessagingService { get; }
        public Guid InternalProcessId { get; }
        public Process Process { get; }
        public string Port { get; }

	    public void WaitAndKillProcess()
	    {
			Timer timer = null;

			timer = new Timer(_ =>
		    {				
			    if (Process.IsRunning())
			    {
					Process.Kill();
			    }			   
				timer.Dispose();
		    }, 
			null, 
			TimeSpan.FromSeconds(30), 
			Timeout.InfiniteTimeSpan);									
	    }
    }
}