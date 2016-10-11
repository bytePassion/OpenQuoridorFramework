using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using OQF.Application.Tournament.ViewModels;

namespace OQF.Application.Tournament.Services
{
    public class TournamentService : ITournamentService
    {

        public void StartTournament(IEnumerable<BotData> contestants)
        {
            StartMatch();
            var t1 = new Thread(CreateServerThread);
            t1.Start();
        }

        private void StartMatch()
        {
            var path = @"..\..\..\OQF.Application.BotVsBot\bin\Debug\OQF.Application.BotVsBot.exe";
            var info =
                new ProcessStartInfo()
                    
                {
                    FileName = path,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(path)
                };
            var process = new Process() { StartInfo = info };
            process.Start();
        }

        private void CreateServerThread()
        {
                using (var socket = new RequestSocket("tcp://localhost:5555"))
                {
                    socket.SendFrame("InitBot");
                    var answer = socket.ReceiveFrameString();
                    if (answer.Equals("Error"))
                    {
                        Console.WriteLine("bot init failed");
                        return;
                    }

                    socket.SendFrame("FirstMove");

                    while (true)
                    {
                        
                    }
                }
        }
    }
}