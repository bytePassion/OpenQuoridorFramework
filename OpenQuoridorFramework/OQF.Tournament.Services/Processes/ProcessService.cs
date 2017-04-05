using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using bytePassion.Lib.FrameworkExtensions;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.Moves;
using OQF.Tournament.Communication.Messages;
using OQF.Tournament.Contracts;
using OQF.Tournament.Services.Messaging;
using OQF.Tournament.Services.Processes.MessageHandler;

namespace OQF.Tournament.Services.Processes
{
	public class ProcessService : DisposingObject, IProcessService
	{
		private readonly IPortService portService;
		private readonly IDictionary<Guid, ProcessData> processes;

		public ProcessService(IPortService portService)
		{
			this.portService = portService;
			processes = new Dictionary<Guid, ProcessData>();
		}

		public Guid CreateBotProcess()
		{
			var port = portService.GetPort();
			var process = CreateWrapperProcessForBot(port);

			process.OutputDataReceived += OutputHandler;
			process.ErrorDataReceived += OutputHandler;

			process.Start();

			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			var id = Guid.NewGuid();
			processes.Add(id, new ProcessData(new MessagingService(port), id, process, port));
			return id;
		}

		// This handler releases and shows stuck messages made by Console.Writes called by external loaded Dlls such as the Bot-Dlls
		private static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
		{
			Debug.WriteLine(outLine.Data);
		}
	

		public void LoadBot(Guid internalProcessId, string dllPath, PlayerType playerSide, Action loadingFinishedCallback)
        {
            if (processes.ContainsKey(internalProcessId))
            {
                new LoadBotHandler(processes[internalProcessId].MessagingService, loadingFinishedCallback);
                var message = new LoadBotRequest(playerSide, dllPath);
                processes[internalProcessId].MessagingService.SendMessage(message);
            }
        }

        public void InitBot(Guid internalProcessId, GameConstraints gameConstraints, PlayerType playerType, Action initFinishedCallback)
        {
            if (processes.ContainsKey(internalProcessId))
            {
                new InitBotHandler(processes[internalProcessId].MessagingService,initFinishedCallback);
                var message = new InitGameRequest(playerType, gameConstraints);
                processes[internalProcessId].MessagingService.SendMessage(message);
            }
        }

        public void DoMove(Guid internalProcessId, QProgress currentProgress, Action<Move> nextBotMove)
        {
            if (processes.ContainsKey(internalProcessId))
            {
                new DoMoveHandler(processes[internalProcessId].MessagingService, nextBotMove);
                var message = new NextMoveRequest(currentProgress);
                processes[internalProcessId].MessagingService.SendMessage(message);				
			}
        }

        public void KillBotProcess(Guid internalProcessId)
        {
			if (processes.ContainsKey(internalProcessId))
			{
				portService.FreePort(processes[internalProcessId].Port);
                new GameFinishedHandler(processes[internalProcessId].MessagingService);
				processes[internalProcessId].MessagingService.SendMessage(new GameFinishedRequest());
				processes[internalProcessId].WaitAndKillProcess();
				processes.Remove(internalProcessId);
			}				
        }

        private static Process CreateWrapperProcessForBot(string port)
        {
            const string path = @"..\..\..\OQF.Application.BotWrapper\bin\Debug\OQF.Tournament.BotWrapper.exe";
            
            return new Process
            {
	            StartInfo = new ProcessStartInfo()
				{
					FileName = path,
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					WorkingDirectory = Path.GetDirectoryName(path),
					Arguments = port
				}
			};
        }

	    protected override void CleanUp()
	    {
		    foreach (var processId in processes.Keys.ToList())
		    {
			    KillBotProcess(processId);
		    }
	    }
    }
}